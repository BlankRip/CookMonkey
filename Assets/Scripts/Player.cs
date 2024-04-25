using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private const string IS_WALKING_ANIMATION_BOOL = "IsWalking";

    [SerializeField] private float moveSpeed = 7.0f;
    [SerializeField] private float rotateSpeed = 7.0f;
    [SerializeField] private Animator animator;

    private void Update()
    {
        Vector2 inputVector = Vector2.zero;
        if(Input.GetKey(KeyCode.W))
        {
            inputVector.y = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            inputVector.y = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputVector.x = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            inputVector.x = -1;
        }
        inputVector.Normalize();

        Vector3 moveDirection = new Vector3(inputVector.x, 0, inputVector.y);
        animator.SetBool(IS_WALKING_ANIMATION_BOOL, moveDirection != Vector3.zero);
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, rotateSpeed * Time.deltaTime);

    }
}
