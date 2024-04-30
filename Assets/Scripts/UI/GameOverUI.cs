using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI recipesDeliveredText;

    private void Start()
    {
        KitchenGameManager.Instance.OnStateChange += KitchenGameManger_OnStateChange;
        gameObject.SetActive(false);
    }

    private void KitchenGameManger_OnStateChange()
    {
        if (KitchenGameManager.Instance.IsGameOver())
        {
            recipesDeliveredText.SetText(DeliveryManager.Instance.GetSuccessfulDeliveriesAmount().ToString());
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
