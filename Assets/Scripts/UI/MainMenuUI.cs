using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button multiplayerButton;
    [SerializeField] private Button singlePlayerButton;
    [SerializeField] private Button quitButton;

    private void Start()
    {
        Time.timeScale = 1.0f;
        multiplayerButton.onClick.AddListener(() => {
            NetworkStatics.PlayingSinglePlayer = false;
            Loader.Load(Loader.Scene.LobbyScene); 
        });
        singlePlayerButton.onClick.AddListener(() => {
            NetworkStatics.PlayingSinglePlayer = true;
            Loader.Load(Loader.Scene.LobbyScene);
        });
        quitButton.onClick.AddListener(() => { Application.Quit(); });
    }
}
