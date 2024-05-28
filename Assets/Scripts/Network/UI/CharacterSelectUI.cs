using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectUI : MonoBehaviour
{
    [SerializeField] private Button readyButton;
    [SerializeField] private Button mainMenuButton;

    private void Start()
    {
        readyButton.onClick.AddListener(() => CharacterSelectReady.Instance.SetPlayerReady());
        mainMenuButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.MainMenuScene);
        });
    }
}
