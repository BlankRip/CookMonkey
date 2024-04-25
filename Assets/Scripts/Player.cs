using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7.0f;
    [SerializeField] private float rotateSpeed = 7.0f;
    [SerializeField] private LayerMask countersLayerMask;
    public bool IsWalking { get; private set; }

    private float playerHight = 2.0f;
    private float playerRadius = 0.7f;
    private float interactDistance = 2.0f;
    private RaycastHit intractionRayHit;
    
    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    private void HandleInteractions()
    {
        if(Physics.Raycast(transform.position, transform.forward, out intractionRayHit, interactDistance, countersLayerMask))
        {
            if(intractionRayHit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                clearCounter.Interact();
            }
        }
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
}