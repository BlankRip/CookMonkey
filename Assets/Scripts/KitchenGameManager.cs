using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenGameManager : MonoBehaviour
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

    private GameState currentState;
    private float countDownToStartTimer = 1.0f;
    private float gamePlayTimer;
    private float gamePlayTimerMax = 300.0f;
    private bool isGamePaused = false;

    private void Start()
    {
        currentState = GameState.WaitingToStart;
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;

        //DEBUG start auto
        currentState = GameState.CountdownToStart;
        OnStateChange?.Invoke();
    }

    private void OnDestroy()
    {
        GameInput.Instance.OnPauseAction -= GameInput_OnPauseAction;
        GameInput.Instance.OnInteractAction -= GameInput_OnInteractAction;
    }

    private void Update()
    {
        switch (currentState)
        {
            case GameState.WaitingToStart:
                
                break;
            case GameState.CountdownToStart:
                countDownToStartTimer -= Time.deltaTime;
                if (countDownToStartTimer < 0.0f)
                {
                    gamePlayTimer = gamePlayTimerMax;
                    currentState = GameState.GamePlaying;
                    OnStateChange?.Invoke();
                }
                break;
            case GameState.GamePlaying:
                gamePlayTimer -= Time.deltaTime;
                if (gamePlayTimer < 0.0f)
                {
                    currentState = GameState.GameOver;
                    OnStateChange?.Invoke();
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
        if (currentState == GameState.WaitingToStart)
        {
            currentState = GameState.CountdownToStart;
            OnStateChange?.Invoke();
        }
    }

    private void GameInput_OnPauseAction()
    {
        if(currentState == GameState.GamePlaying)
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (!isGamePaused)
        {
            Time.timeScale = 0.0f;
            isGamePaused = true;
        }
        else
        {
            Time.timeScale = 1.0f;
            isGamePaused = false;
        }
        OnGamePauseToggled?.Invoke(isGamePaused);
    }

    public bool IsGamePlaying()
    {
        return currentState == GameState.GamePlaying;
    }

    public bool IsCountDownToStartActive()
    {
        return currentState == GameState.CountdownToStart;
    }

    public float GetCountDownToStartTimer()
    {
        return countDownToStartTimer;
    }

    public bool IsGameOver()
    {
        return currentState == GameState.GameOver;
    }

    public float GetGamePlayingTimerNormalized()
    {
        return gamePlayTimer / gamePlayTimerMax;
    }
}
