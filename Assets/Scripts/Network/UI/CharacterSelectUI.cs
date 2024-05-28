using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectUI : MonoBehaviour
{
    [SerializeField] private Button readyButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private TextMeshProUGUI lobbyNameText;
    [SerializeField] private TextMeshProUGUI lobbyCodeText;
    [SerializeField] private Button copyToClipboardButton;

    private void Start()
    {
        readyButton.onClick.AddListener(() => CharacterSelectReady.Instance.SetPlayerReady());
        mainMenuButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.MainMenuScene);
        });

        Lobby lobby =  KitchenGameLobby.Instance.JoinedLobby;
        lobbyNameText.SetText($"Lobby Name: {lobby.Name}");
        lobbyCodeText.SetText($"Lobby Code: {lobby.LobbyCode}");
        copyToClipboardButton.onClick.AddListener(() => GUIUtility.systemCopyBuffer = lobby.LobbyCode);
    }
}
