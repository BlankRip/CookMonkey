using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnWarningUI : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    private float burntShowProgressAmount = 0.5f;

    private void Start()
    {
        stoveCounter.OnProgressChanged += Stove_OnProgressChanged;
        Hide();
    }

    private void OnDestroy()
    {
        stoveCounter.OnProgressChanged -= Stove_OnProgressChanged;
    }

    private void Stove_OnProgressChanged(float progressAmountNormalized)
    {
        if(stoveCounter.IsFried() && progressAmountNormalized > burntShowProgressAmount)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
