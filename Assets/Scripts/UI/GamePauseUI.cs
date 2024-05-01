using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private OptionsUI optionsUI;

    private void Start()
    {
        mainMenuButton.onClick.AddListener(() => { Loader.Load(Loader.Scene.MainMenuScene); });
        resumeButton.onClick.AddListener(() => { KitchenGameManager.Instance.TogglePause(); });
        optionsButton?.onClick.AddListener(() => { optionsUI?.Show(this); Hide(); });
        KitchenGameManager.Instance.OnGamePauseToggled += KitchenGameManger_OnGamePauseToggled;
        Hide();
    }

    private void OnDestroy()
    {
        KitchenGameManager.Instance.OnGamePauseToggled -= KitchenGameManger_OnGamePauseToggled;
    }

    private void KitchenGameManger_OnGamePauseToggled(bool isPaused)
    {
        if(isPaused)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
