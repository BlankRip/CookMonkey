using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.VisualScripting;

public class ConnectionResponseMessageUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Button closeButton;

    private void Start()
    {
        KitchenGameMultiplayer.Instance.OnFailedToJoinGame += KitchenGameMultiplayer_OnFailedToJoinGame;
        closeButton.onClick.AddListener(() => this.gameObject.SetActive(false));

        this.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        KitchenGameMultiplayer.Instance.OnFailedToJoinGame -= KitchenGameMultiplayer_OnFailedToJoinGame;
    }

    private void KitchenGameMultiplayer_OnFailedToJoinGame()
    {
        this.gameObject.SetActive(true);

        messageText.SetText(NetworkManager.Singleton.DisconnectReason);
        if(messageText.text == "")
        {
            messageText.SetText("Failed to connect");
        }
    }
}
