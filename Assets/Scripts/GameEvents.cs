using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents Instance { get; private set; }
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    /// <summary>
    /// Player is the sender, ClearCounter is the counter object it self
    /// </summary>
    public event Action<Player, ClearCounter> OnSelectedCounterChange;

    public void InvokeOnSelectedCounter(Player sender, ClearCounter selectedCounter)
    {
        OnSelectedCounterChange?.Invoke(sender, selectedCounter);
    }

}
