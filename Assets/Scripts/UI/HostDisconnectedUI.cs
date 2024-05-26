using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HostDisconnectedUI : MonoBehaviour
{
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button mainMenuButton;

    private void Start()
    {
        playAgainButton.onClick.AddListener(() => { Loader.Load(Loader.Scene.GameScene); });
        mainMenuButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.MainMenuScene);
        });
        gameObject.SetActive(false);

        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
    {
        if(clientId == NetworkManager.ServerClientId)
        {
            gameObject.SetActive(true);
        }
    }
}
