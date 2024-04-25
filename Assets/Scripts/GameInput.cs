using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

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

    InputAction playerMove;
    private void Start()
    {
        playerMove = playerInput.actions["Move"];
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerMove.ReadValue<Vector2>();
        inputVector.Normalize();
        return inputVector;
    }
}
