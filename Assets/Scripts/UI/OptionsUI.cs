using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    [System.Serializable]
    private struct KeyBindButton
    {
        public Button TheButton;
        public TextMeshProUGUI TextInButton;
    }

    [SerializeField] private Button soundEffectsButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private TextMeshProUGUI soundEffectsText;
    [SerializeField] private TextMeshProUGUI musicText;

    [Space]
    [Header("Key Rebind")]
    [SerializeField] private GameObject pressToRebindInfoPanel;
    [SerializeField] private KeyBindButton moveUpButton;
    [SerializeField] private KeyBindButton moveDownButton;
    [SerializeField] private KeyBindButton moveLeftButton;
    [SerializeField] private KeyBindButton moveRightButton;
    [SerializeField] private KeyBindButton InteractButton;
    [SerializeField] private KeyBindButton InteractAltButton;
    [SerializeField] private KeyBindButton pauseButton;


    private GamePauseUI gamePauseUI;

    private void Start()
    {
        soundEffectsButton.onClick.AddListener(() => {
            SoundManager.Instance.ChangeVolume();
            UpdateVisuals();
        });
        musicButton.onClick.AddListener(() => { 
            MusicManager.Instance.ChangeVolume();
            UpdateVisuals();
        });
        closeButton.onClick.AddListener(() => { 
            Hide();
            gamePauseUI.Show();
        });
        KitchenGameManager.Instance.OnGamePauseToggled += KitchenGameManger_OnGamePauseToggled;

        SetUpRebindButtons();
        pressToRebindInfoPanel.SetActive(false);

        UpdateVisuals();
    }

    private void OnDestroy()
    {
        KitchenGameManager.Instance.OnGamePauseToggled -= KitchenGameManger_OnGamePauseToggled;
    }

    private void KitchenGameManger_OnGamePauseToggled(bool isPaused)
    {
        if (!isPaused)
        {
            Hide();
        }
    }

    private void UpdateVisuals()
    {
        soundEffectsText.SetText($"Sound Effects: {(int)(SoundManager.Instance.GetVolume() * 10)}");
        musicText.SetText($"Music: {(int)(MusicManager.Instance.GetVolume() * 10)}");

        moveUpButton.TextInButton.SetText(GameInput.Instance.GetBindingText(GameInput.Binding.Up));
        moveDownButton.TextInButton.SetText(GameInput.Instance.GetBindingText(GameInput.Binding.Down));
        moveLeftButton.TextInButton.SetText(GameInput.Instance.GetBindingText(GameInput.Binding.Left));
        moveRightButton.TextInButton.SetText(GameInput.Instance.GetBindingText(GameInput.Binding.Right));
        InteractButton.TextInButton.SetText(GameInput.Instance.GetBindingText(GameInput.Binding.Interact));
        InteractAltButton.TextInButton.SetText(GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlt));
        pauseButton.TextInButton.SetText(GameInput.Instance.GetBindingText(GameInput.Binding.Pause));
    }

    private void SetUpRebindButtons()
    {
        moveUpButton.TheButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Up); });
        moveDownButton.TheButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Down); });
        moveLeftButton.TheButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Left); });
        moveRightButton.TheButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Right); });
        InteractButton.TheButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Interact); });
        InteractAltButton.TheButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.InteractAlt); });
        pauseButton.TheButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Pause); });
    }

    private void RebindBinding(GameInput.Binding binding)
    {
        pressToRebindInfoPanel.SetActive(true);
        GameInput.Instance.RebindBinding(binding, OnRebindingComplete);
    }

    private void OnRebindingComplete()
    {
        pressToRebindInfoPanel.SetActive(false);
        UpdateVisuals();
    }

    public void Show(GamePauseUI caller)
    {
        gameObject.SetActive(true);
        gamePauseUI = caller;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
