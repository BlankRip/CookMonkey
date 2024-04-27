using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private CuttingCounter cuttingCounter;
    [SerializeField] private Image barImage;

    private void Start()
    {
        cuttingCounter.OnProgressChanged += OnProgressChanged;
        barImage.fillAmount = 0;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        cuttingCounter.OnProgressChanged -= OnProgressChanged;
    }

    private void OnProgressChanged(float newNormalizedProgress)
    {
        barImage.fillAmount = newNormalizedProgress;
        if(newNormalizedProgress <= 0 || newNormalizedProgress >= 1)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
