using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;
    private int countdownNumber;
    private int previousCountDownNumber;

    private void Start()
    {
        KitchenGameManager.Instance.OnStateChange += KitchenGameManger_OnStateChange;
        gameObject.SetActive(false);
    }

    private void KitchenGameManger_OnStateChange()
    {
        if(KitchenGameManager.Instance.IsCountDownToStartActive())
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        countdownNumber = Mathf.CeilToInt(KitchenGameManager.Instance.GetCountDownToStartTimer());
        countdownText.SetText(countdownNumber.ToString());

        if(countdownNumber != previousCountDownNumber)
        {
            previousCountDownNumber = countdownNumber;
            SoundManager.Instance.PlayeCountdownSound();
        }
    }
}
