using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene
    {
        MainMenuScene,
        GameScene,
        LoadingScene,
        LobbyScene,
        CharacterSelectScene
    }
    private static Scene targetScene;

    public static void Load(Scene scene)
    {
        targetScene = scene;
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }

    public static void LoadNetwork(Scene scene)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(scene.ToString(), LoadSceneMode.Single);
    }

    public static void LoaderCallback()
    {
        SceneManager.LoadScene(targetScene.ToString());
    }
}
