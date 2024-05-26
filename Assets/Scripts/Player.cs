using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Netcode;

public class Player : NetworkBehaviour, IKitchenObjectParent
{
    [SerializeField] private float moveSpeed = 7.0f;
    [SerializeField] private float rotateSpeed = 7.0f;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private LayerMask collisionLayerMask;
    [SerializeField] private Transform kitchenObjectHoldParent;
    [SerializeField] private Vector3[] spawnPositions;
    public bool IsWalking { get; private set; }

    private float playerHight = 2.0f;
    private float playerRadius = 0.7f;
    private float interactDistance = 1.5f;
    private RaycastHit intractionRayHit;

    private BaseCounter selectedCounter;
    private KitchenObject kitchenObjectHeld;

    private void Start()
    {
        if (!IsOwner)
            return;

        GameInput.Instance.OnInteractAction += OnInteractAction;
        GameInput.Instance.OnInteractAlternateAction += OnInteractAlternateAction;
    }

    public override void OnNetworkSpawn()
    {
        transform.position = spawnPositions[(int)OwnerClientId];

        if(IsServer)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
        }
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
    {
        if(clientId == OwnerClientId && HasKitchenObject())
        {
            KitchenObject.DestroyKitchenObject(GetKitchenObjectHeld());
        }
    }

    private void OnDestroy()
    {
        if (!IsOwner)
            return;

        GameInput.Instance.OnInteractAction -= OnInteractAction;
        GameInput.Instance.OnInteractAlternateAction -= OnInteractAlternateAction;
    }

    private void Update()
    {
        if(!IsOwner || KitchenGameManager.Instance.IsGamePaused)
            return;

        HandleMovement();
        HandleInteractions();
    }

    private void HandleInteractions()
    {
        if (Physics.Raycast(transform.position, transform.forward, out intractionRayHit, interactDistance, countersLayerMask))
        {
            if(intractionRayHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if(selectedCounter != baseCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }

    private void OnInteractAction()
    {
        if(!KitchenGameManager.Instance.IsGamePlaying())
        {
            return;
        }
        selectedCounter?.Interact(this);
    }

    private void OnInteractAlternateAction()
    {
        selectedCounter?.InteractAlternate(this);
    }

    private void SetSelectedCounter(BaseCounter counterToSelect)
    {
        selectedCounter = counterToSelect;
        GameEvents.Instance.InvokeOnSelectedCounter(this, selectedCounter);
    }

    private void HandleMovement()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        float moveDistance = moveSpeed * Time.deltaTime;
        bool canMove = false;
        if (inputVector != Vector2.zero)
        {
            canMove = !Physics.BoxCast(transform.position, Vector3.one * playerRadius, moveDir, Quaternion.identity, moveDistance, collisionLayerMask);
            HandleDiagonalCollisionDetection(ref canMove, ref moveDir);
        }

        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }
        transform.forward = Vector3.Slerp(transform.forward, moveDir, rotateSpeed * Time.deltaTime);
        IsWalking = moveDir != Vector3.zero;
    }

    private void HandleDiagonalCollisionDetection(ref bool canMove, ref Vector3 moveDir)
    {
        bool accountForX = moveDir.x < -0.5f || moveDir.x > 0.5f;
        bool accountForZ = moveDir.z < -0.5f || moveDir.z > 0.5f;
        if (!canMove && (accountForX && accountForZ))
        {
            float moveDistance = moveSpeed * Time.deltaTime;
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.BoxCast(transform.position, Vector3.one * playerRadius, moveDirX, Quaternion.identity, moveDistance, collisionLayerMask);
            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = !Physics.BoxCast(transform.position, Vector3.one * playerRadius, moveDirZ, Quaternion.identity, moveDistance, collisionLayerMask);
                if (canMove)
                {
                    moveDir = moveDirZ;
                }
            }
        }
    }

    #region IKitchenObjectParent
    public Transform GetKitchenObjectParentTransform()
    {
        return kitchenObjectHoldParent;
    }

    public void SetKitchenObjectHeld(KitchenObject kitchenObject)
    {
        kitchenObjectHeld = kitchenObject;
        GameEvents.Instance.InvokeOnPlayerPickedSomething(this);
    }

    public KitchenObject GetKitchenObjectHeld()
    {
        return kitchenObjectHeld;
    }

    public void ClearKitchenbjectHeld()
    {
        kitchenObjectHeld = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObjectHeld != null;
    }

    public NetworkObject GetNetworkObject()
    {
        return NetworkObject;
    }
    #endregion
}
