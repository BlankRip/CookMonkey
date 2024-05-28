using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button createLobbyButton;
    [SerializeField] private Button quickJoinButton;
    [SerializeField] private LobbyCreateUI lobbyCreateUI;
    [Space]
    [SerializeField] private TMP_InputField lobbyCodeInputField;
    [SerializeField] private Button codeJoinButton;

    private void Start()
    {
        mainMenuButton.onClick.AddListener(() => Loader.Load(Loader.Scene.MainMenuScene));
        createLobbyButton.onClick.AddListener(() => lobbyCreateUI.Show()); 
        quickJoinButton.onClick.AddListener(() => KitchenGameLobby.Instance.QuickJoin());
        codeJoinButton.onClick.AddListener(() => JoinLobbyByCode());
    }

    private void JoinLobbyByCode()
    {
        if(!string.IsNullOrEmpty(lobbyCodeInputField.text))
        {
            KitchenGameLobby.Instance.JoinLobbyWithCode(lobbyCodeInputField.text);
        }
    }
}
