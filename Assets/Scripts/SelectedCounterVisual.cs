using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter counter;
    [SerializeField] private GameObject[] selectedVisualObjectArray;

    private void Start()
    {
        GameEvents.Instance.OnSelectedCounterChange += OnSelectedCounterChange;
    }

    private void OnDestroy()
    {
        GameEvents.Instance.OnSelectedCounterChange -= OnSelectedCounterChange;
    }

    private void OnSelectedCounterChange(Player sender, BaseCounter selectedCounter)
    {
        if(counter == selectedCounter)
        {
            foreach (GameObject go in selectedVisualObjectArray)
            {
                go.SetActive(true);
            }
        }
        else
        {
            foreach (GameObject go in selectedVisualObjectArray)
            {
                go.SetActive(false);
            }
        }
    }
}
