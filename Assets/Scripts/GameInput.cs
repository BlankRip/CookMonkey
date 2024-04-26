using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance;
    private PlayerInput playerInput;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            playerInput = GetComponent<PlayerInput>();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public event Action OnInteractAction;

    private InputAction playerMove;
    private InputAction interact;
    private void Start()
    {
        playerMove = playerInput.actions["Move"];
        interact = playerInput.actions["Interact"];
        interact.performed += InteractPerformed;
    }

    private void OnDestroy()
    {
        interact.performed -= InteractPerformed;
    }

    private void InteractPerformed(InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke();
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerMove.ReadValue<Vector2>();
        inputVector.Normalize();
        return inputVector;
    }
}
