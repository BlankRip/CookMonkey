using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance;
    private PlayerInput playerInput;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            playerInput = GetComponent<PlayerInput>();
            if(PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS_KEY))
            {
                playerInput.actions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS_KEY));
            }
            Initilize();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public enum Binding
    {
        Up, Down, Left, Right, Interact, InteractAlt, Pause, GamePad_Interact, GamePad_InteractAlt, GamePad_Pause
    }

    private const string PLAYER_PREFS_BINDINGS_KEY = "InputBindings"; 

    public event Action OnInteractAction;
    public event Action OnInteractAlternateAction;
    public event Action OnPauseAction;

    private InputActionMap defaultActionMap;
    private InputAction playerMove;
    private InputAction interact;
    private InputAction interactAlternate;
    private InputAction pause;
    private void Initilize()
    {
        defaultActionMap = playerInput.currentActionMap;
        playerMove = playerInput.actions["Move"];
        interact = playerInput.actions["Interact"];
        interactAlternate = playerInput.actions["InteractAlternate"];
        pause = playerInput.actions["Pause"];
        interact.performed += InteractPerformed;
        interactAlternate.performed += InteractAlternatePerformed;
        pause.performed += PausePerformed;
    }

    private void OnDestroy()
    {
        interact.performed -= InteractPerformed;
        interactAlternate.performed -= InteractAlternatePerformed;
        pause.performed -= PausePerformed;
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

    private void PausePerformed(InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke();
    }

    public string GetBindingText(Binding binding)
    {
        switch (binding)
        {
            case Binding.Up:
                return playerMove.bindings[1].ToDisplayString();
            case Binding.Down:
                return playerMove.bindings[2].ToDisplayString();
            case Binding.Left:
                return playerMove.bindings[3].ToDisplayString();
            case Binding.Right:
                return playerMove.bindings[4].ToDisplayString();
            case Binding.Interact:
                return interact.bindings[0].ToDisplayString();
            case Binding.InteractAlt:
                return interactAlternate.bindings[0].ToDisplayString();
            case Binding.Pause:
                return pause.bindings[0].ToDisplayString();
            case Binding.GamePad_Interact:
                return interact.bindings[1].ToDisplayString();
            case Binding.GamePad_InteractAlt:
                return interactAlternate.bindings[1].ToDisplayString();
            case Binding.GamePad_Pause:
                return pause.bindings[1].ToDisplayString();
        }
        return "N/D";
    }

    public void RebindBinding(Binding binding, Action onActionRebound)
    {
        defaultActionMap.Disable();
        InputAction inputAction = playerMove;
        int bindingIndex = 0;

        switch (binding)
        {
            case Binding.Up:
                inputAction = playerMove;
                bindingIndex = 1;
                break;
            case Binding.Down:
                inputAction = playerMove;
                bindingIndex = 2;
                break;
            case Binding.Left:
                inputAction = playerMove;
                bindingIndex = 3;
                break;
            case Binding.Right:
                inputAction = playerMove;
                bindingIndex = 4;
                break;
            case Binding.Interact:
                inputAction = interact;
                bindingIndex = 0;
                break;
            case Binding.InteractAlt:
                inputAction = interactAlternate;
                bindingIndex = 0;
                break;
            case Binding.Pause:
                inputAction = pause;
                bindingIndex = 0;
                break;
            case Binding.GamePad_Interact:
                inputAction = interact;
                bindingIndex = 1;
                break;
            case Binding.GamePad_InteractAlt:
                inputAction = interactAlternate;
                bindingIndex = 1;
                break;
            case Binding.GamePad_Pause:
                inputAction = pause;
                bindingIndex = 1;
                break;
        }

        inputAction.PerformInteractiveRebinding(bindingIndex).OnComplete((callback) =>
        {
            callback.Dispose();
            defaultActionMap.Enable();
            onActionRebound?.Invoke();

            PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS_KEY, playerInput.actions.SaveBindingOverridesAsJson());
        }).Start();
    }
}
