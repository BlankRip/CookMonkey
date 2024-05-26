using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingForOtherPlayersUI : MonoBehaviour
{
    private void Start()
    {
        KitchenGameManager.Instance.OnLocalPlayerReadyChanged += KitchenGameManager_OnLocalPlayerReadyChanged;
        KitchenGameManager.Instance.OnStateChange += KitchenGameManager_OnStateChanged;
        this.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        KitchenGameManager.Instance.OnLocalPlayerReadyChanged -= KitchenGameManager_OnLocalPlayerReadyChanged;
        KitchenGameManager.Instance.OnStateChange -= KitchenGameManager_OnStateChanged;
    }

    private void KitchenGameManager_OnStateChanged()
    {
        if (KitchenGameManager.Instance.IsCountDownToStartActive())
        {
            this.gameObject.SetActive(false);
        }
    }

    private void KitchenGameManager_OnLocalPlayerReadyChanged()
    {
        if (KitchenGameManager.Instance.IsLocalPlayerReady)
        {
            this.gameObject.SetActive(true);
        }
    }
}
