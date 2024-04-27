using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject IHasProgressGameObject;
    [SerializeField] private Image barImage;
    private IHasProgress hasProgress;

    private void Start()
    {
        hasProgress = IHasProgressGameObject.GetComponent<IHasProgress>();
        if(hasProgress != null)
        {
            hasProgress.OnProgressChanged += OnProgressChanged;
            barImage.fillAmount = 0;
            gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("The object provided does not implement IHasProgress");
        }
    }

    private void OnDestroy()
    {
        hasProgress.OnProgressChanged -= OnProgressChanged;
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
