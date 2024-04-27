using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] private float moveSpeed = 7.0f;
    [SerializeField] private float rotateSpeed = 7.0f;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldParent;
    public bool IsWalking { get; private set; }

    private float playerHight = 2.0f;
    private float playerRadius = 0.7f;
    private float interactDistance = 1.5f;
    private RaycastHit intractionRayHit;

    private BaseCounter selectedCounter;
    private KitchenObject kitchenObjectHeld;

    private void Start()
    {
        GameInput.Instance.OnInteractAction += OnInteractAction;
        GameInput.Instance.OnInteractAlternateAction += OnInteractAlternateAction;
    }

    private void OnDestroy()
    {
        GameInput.Instance.OnInteractAction -= OnInteractAction;
        GameInput.Instance.OnInteractAlternateAction -= OnInteractAlternateAction;
    }

    private void Update()
    {
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
            canMove = !Physics.CapsuleCast(transform.position, transform.position + (Vector3.up * playerHight), playerRadius, moveDir, moveDistance);
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
        if (!canMove && (moveDir.x != 0 && moveDir.z != 0))
        {
            float moveDistance = moveSpeed * Time.deltaTime;
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + (Vector3.up * playerHight), playerRadius, moveDirX, moveDistance);
            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + (Vector3.up * playerHight), playerRadius, moveDirZ, moveDistance);
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
    #endregion
}
