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

    private GameState currentState;
    private float waitingToStartTimer = 1.0f;
    private float countDownToStartTimer = 3.0f;
    private float gamePlayTimer;
    private float gamePlayTimerMax = 10.0f;

    private void Start()
    {
        currentState = GameState.WaitingToStart;
    }

    private void Update()
    {
        switch (currentState)
        {
            case GameState.WaitingToStart:
                waitingToStartTimer -= Time.deltaTime;
                if(waitingToStartTimer < 0.0f)
                {
                    currentState = GameState.CountdownToStart;
                    OnStateChange?.Invoke();
                }
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
