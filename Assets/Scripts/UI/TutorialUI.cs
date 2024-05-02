using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moveUpKeyText;
    [SerializeField] private TextMeshProUGUI moveDownKeyText;
    [SerializeField] private TextMeshProUGUI moveLeftKeyText;
    [SerializeField] private TextMeshProUGUI moveRightKeyText;
    [SerializeField] private TextMeshProUGUI interactKeyText;
    [SerializeField] private TextMeshProUGUI interactAltKeyText;
    [SerializeField] private TextMeshProUGUI pauseKeyText;

    [SerializeField] private TextMeshProUGUI gamePadInteractKeyText;
    [SerializeField] private TextMeshProUGUI gamePadInteractAltKeyText;
    [SerializeField] private TextMeshProUGUI gamePadPauseKeyText;

    private bool removedOnStateChangedListner;

    private void Start()
    {
        GameInput.Instance.OnRebind += UpdateVisuals;
        KitchenGameManager.Instance.OnStateChange += KitchenGameManger_OnStateChanged;
        removedOnStateChangedListner = false;
        UpdateVisuals();
    }

    private void OnDestroy()
    {
        GameInput.Instance.OnRebind -= UpdateVisuals;
        if(!removedOnStateChangedListner)
        {
            KitchenGameManager.Instance.OnStateChange -= KitchenGameManger_OnStateChanged;
        }
    }

    private void UpdateVisuals()
    {
        moveUpKeyText.SetText(GameInput.Instance.GetBindingText(GameInput.Binding.Up));
        moveDownKeyText.SetText(GameInput.Instance.GetBindingText(GameInput.Binding.Down));
        moveLeftKeyText.SetText(GameInput.Instance.GetBindingText(GameInput.Binding.Left));
        moveRightKeyText.SetText(GameInput.Instance.GetBindingText(GameInput.Binding.Right));
        interactKeyText.SetText(GameInput.Instance.GetBindingText(GameInput.Binding.Interact));
        interactAltKeyText.SetText(GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlt));
        pauseKeyText.SetText(GameInput.Instance.GetBindingText(GameInput.Binding.Pause));

        gamePadInteractKeyText.SetText(GameInput.Instance.GetBindingText(GameInput.Binding.GamePad_Interact));
        gamePadInteractAltKeyText.SetText(GameInput.Instance.GetBindingText(GameInput.Binding.GamePad_InteractAlt));
        gamePadPauseKeyText.SetText(GameInput.Instance.GetBindingText(GameInput.Binding.GamePad_Pause));
    }

    private void KitchenGameManger_OnStateChanged()
    {
        if (KitchenGameManager.Instance.IsCountDownToStartActive())
        {
            this.gameObject.SetActive(false);
            GameInput.Instance.OnInteractAction -= KitchenGameManger_OnStateChanged;
            removedOnStateChangedListner = true;
        }
    }
}
