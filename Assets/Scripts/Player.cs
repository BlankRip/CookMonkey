using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7.0f;
    [SerializeField] private float rotateSpeed = 7.0f;
    [SerializeField] private float playerHight = 2.0f;
    [SerializeField] private float playerRadius = 0.7f;
    public bool IsWalking { get; private set; }

    private void Update()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();

        Vector3 moveDirection = new Vector3(inputVector.x, 0, inputVector.y);
        float moveDistance = moveSpeed * Time.deltaTime;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + (Vector3.up * playerHight), playerRadius, moveDirection, moveDistance);
        HandleDiagonalCollisionDetection(ref canMove, ref moveDirection);

        if (canMove)
        {
            transform.position += moveDirection * moveDistance;
            transform.forward = Vector3.Slerp(transform.forward, moveDirection, rotateSpeed * Time.deltaTime);
        }
        IsWalking = moveDirection != Vector3.zero;
    }

    private void HandleDiagonalCollisionDetection(ref bool canMove, ref Vector3 moveDirection)
    {
        if (!canMove && (moveDirection.x != 0 && moveDirection.z != 0))
        {
            Debug.Log("Diagonal");
            float moveDistance = moveSpeed * Time.deltaTime;
            Vector3 moveDirX = new Vector3(moveDirection.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + (Vector3.up * playerHight), playerRadius, moveDirX, moveDistance);
            if (canMove)
            {
                moveDirection = moveDirX;
            }
            else
            {
                Vector3 moveDirZ = new Vector3(0, 0, moveDirection.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + (Vector3.up * playerHight), playerRadius, moveDirZ, moveDistance);
                if (canMove)
                {
                    moveDirection = moveDirZ;
                }
            }
        }
    }
}
