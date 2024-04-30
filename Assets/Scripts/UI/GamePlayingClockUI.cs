using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayingClockUI : MonoBehaviour
{
    [SerializeField] private Image clockImage;

    private void Start()
    {
            clockImage.fillAmount = 1.0f;        
    }

    private void Update()
    {
        if(KitchenGameManager.Instance.IsGamePlaying())
        {
            clockImage.fillAmount = KitchenGameManager.Instance.GetGamePlayingTimerNormalized();
        }
    }
}
