using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private ClearCounter clearCounter;
    [SerializeField] private GameObject selectedVisual;

    private void Start()
    {
        GameEvents.Instance.OnSelectedCounterChange += OnSelectedCounterChange;
    }

    private void OnDestroy()
    {
        GameEvents.Instance.OnSelectedCounterChange -= OnSelectedCounterChange;
    }

    private void OnSelectedCounterChange(Player sender, ClearCounter selectedCounter)
    {
        if(clearCounter == selectedCounter)
        {
            selectedVisual.SetActive(true);
        }
        else
        {
            selectedVisual.SetActive(false);
        }
    }
}
