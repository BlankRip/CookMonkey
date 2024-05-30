using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyCreateUI : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Button createPublicButton;
    [SerializeField] private Button createPrivateButton;
    [SerializeField] private TMP_InputField lobbyNameInputField;

    private const string NO_LOBBY_NAME_PROVIDED = "No Name Given";

    private void Start()
    {
        closeButton.onClick.AddListener(() => this.gameObject.SetActive(false));
        createPublicButton.onClick.AddListener(() => CreateLobby(false));
        createPrivateButton.onClick.AddListener(() => CreateLobby(true));
    }

    private void CreateLobby(bool isPrivate)
    {
        if(!string.IsNullOrEmpty(lobbyNameInputField.text))
        {
            KitchenGameLobby.Instance.CreateLobby(lobbyNameInputField.text, isPrivate);
        }
        else
        {
            KitchenGameLobby.Instance.CreateLobby(NO_LOBBY_NAME_PROVIDED, isPrivate);
        }
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
        createPublicButton.Select();
    }
}
