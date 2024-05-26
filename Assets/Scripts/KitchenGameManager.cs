using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;

public class KitchenGameManager : NetworkBehaviour
{
    public static KitchenGameManager Instance;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private enum GameState { WaitingToStart, CountdownToStart, GamePlaying, GameOver }

    public event Action OnStateChange;
    public event Action<bool> OnGamePauseToggled;
    public event Action OnLocalPlayerReadyChanged;
    public bool IsLocalPlayerReady { get; private set; }
    private Dictionary<ulong, bool> playerReadyDictionary = new Dictionary<ulong, bool>();

    private NetworkVariable<GameState> currentState = new NetworkVariable<GameState>(GameState.WaitingToStart);
    private NetworkVariable<float> countDownToStartTimer = new NetworkVariable<float>(3.0f);
    private NetworkVariable<float> gamePlayTimer = new NetworkVariable<float>(0.0f);
    private float gamePlayTimerMax = 300.0f;
    public bool IsGamePaused { get; private set; }


    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
    }

    public override void OnNetworkSpawn()
    {
        currentState.OnValueChanged += CurrentStateOnValueChange;
    }

    private void CurrentStateOnValueChange(GameState previousState, GameState newState)
    {
        Debug.Log(currentState.Value);
        OnStateChange?.Invoke();
    }

    public override void OnNetworkDespawn()
    {
        currentState.OnValueChanged -= CurrentStateOnValueChange;
        GameInput.Instance.OnPauseAction -= GameInput_OnPauseAction;
        GameInput.Instance.OnInteractAction -= GameInput_OnInteractAction;
    }

    private void Update()
    {
        if (!IsServer)
            return;

        switch (currentState.Value)
        {
            case GameState.WaitingToStart:
                
                break;
            case GameState.CountdownToStart:
                countDownToStartTimer.Value -= Time.deltaTime;
                if (countDownToStartTimer.Value < 0.0f)
                {
                    gamePlayTimer.Value = gamePlayTimerMax;
                    currentState.Value = GameState.GamePlaying;
                }
                break;
            case GameState.GamePlaying:
                gamePlayTimer.Value -= Time.deltaTime;
                if (gamePlayTimer.Value < 0.0f)
                {
                    currentState.Value = GameState.GameOver;
                }
                break;
            case GameState.GameOver:
                break;
            default:
                break;
        }
    }

    private void GameInput_OnInteractAction()
    {
        if (currentState.Value == GameState.WaitingToStart)
        {
            IsLocalPlayerReady = true;
            OnLocalPlayerReadyChanged?.Invoke();
            SetPlayerReadyServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;

        bool allClientsReady = true;
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (!playerReadyDictionary.ContainsKey(clientId) || playerReadyDictionary[clientId] == false)
            {
                allClientsReady = false;
                break;
            }
        }
        if (allClientsReady)
        {
            currentState.Value = GameState.CountdownToStart;
        }
    }

    private void GameInput_OnPauseAction()
    {
        if(currentState.Value == GameState.GamePlaying)
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (!IsGamePaused)
        {
            if(NetworkStatics.PlayingSinglePlayer)
            {
                Time.timeScale = 0.0f;
            }
            IsGamePaused = true;
        }
        else
        {
            Time.timeScale = 1.0f;
            IsGamePaused = false;
        }
        OnGamePauseToggled?.Invoke(IsGamePaused);
    }

    public bool IsGamePlaying()
    {
        return currentState.Value == GameState.GamePlaying;
    }

    public bool IsCountDownToStartActive()
    {
        return currentState.Value == GameState.CountdownToStart;
    }

    public float GetCountDownToStartTimer()
    {
        return countDownToStartTimer.Value;
    }

    public bool IsGameOver()
    {
        return currentState.Value == GameState.GameOver;
    }

    public float GetGamePlayingTimerNormalized()
    {
        return gamePlayTimer.Value / gamePlayTimerMax;
    }
}
