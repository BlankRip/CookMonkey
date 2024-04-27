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
    public event Action OnInteractAlternateAction;

    private InputAction playerMove;
    private InputAction interact;
    private InputAction interactAlternate;
    private void Start()
    {
        playerMove = playerInput.actions["Move"];
        interact = playerInput.actions["Interact"];
        interactAlternate = playerInput.actions["InteractAlternate"];
        interact.performed += InteractPerformed;
        interactAlternate.performed += InteractAlternatePerformed;
    }

    private void OnDestroy()
    {
        interact.performed -= InteractPerformed;
        interactAlternate.performed -= InteractAlternatePerformed;
    }

    private void InteractAlternatePerformed(InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke();
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
