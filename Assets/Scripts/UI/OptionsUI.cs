using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    [SerializeField] private Button soundEffectsButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private TextMeshProUGUI soundEffectsText;
    [SerializeField] private TextMeshProUGUI musicText;

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
