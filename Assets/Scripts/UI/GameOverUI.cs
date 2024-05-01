using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI recipesDeliveredText;
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button mainMenuButton;

    private void Start()
    {
        playAgainButton.onClick.AddListener(() => { Loader.Load(Loader.Scene.GameScene); });
        mainMenuButton.onClick.AddListener(() => { Loader.Load(Loader.Scene.MainMenuScene); });
        KitchenGameManager.Instance.OnStateChange += KitchenGameManger_OnStateChange;
        gameObject.SetActive(false);
    }

    private void KitchenGameManger_OnStateChange()
    {
        if (KitchenGameManager.Instance.IsGameOver())
        {
            recipesDeliveredText.SetText(DeliveryManager.Instance.GetSuccessfulDeliveriesAmount().ToString());
            gameObject.SetActive(true);
            playAgainButton.Select();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
