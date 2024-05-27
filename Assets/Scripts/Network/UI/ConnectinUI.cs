using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectinUI : MonoBehaviour
{
    private void Start()
    {
        KitchenGameMultiplayer.Instance.OnTryingToJoinGame += KitchenGameMultiplayer_OnTryingToJoinGame;
        KitchenGameMultiplayer.Instance.OnFailedToJoinGame += KitchenGameMultiplayer_OnFailedToJoinGame;

        this.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        KitchenGameMultiplayer.Instance.OnTryingToJoinGame -= KitchenGameMultiplayer_OnTryingToJoinGame;
        KitchenGameMultiplayer.Instance.OnFailedToJoinGame -= KitchenGameMultiplayer_OnFailedToJoinGame;
    }

    private void KitchenGameMultiplayer_OnTryingToJoinGame()
    {
        this.gameObject.SetActive(true);
    }

    private void KitchenGameMultiplayer_OnFailedToJoinGame()
    {
        this.gameObject.SetActive(false);
    }
}
