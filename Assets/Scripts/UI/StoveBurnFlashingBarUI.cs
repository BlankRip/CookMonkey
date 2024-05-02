using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnFlashingBarUI : MonoBehaviour
{
    private const string IS_FLASHING_ANIMATOR_KEY = "IsFlashing";

    [SerializeField] private StoveCounter stoveCounter;
    private Animator animator;
    private float burntShowProgressAmount = 0.5f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        stoveCounter.OnProgressChanged += Stove_OnProgressChanged;
        animator.SetBool(IS_FLASHING_ANIMATOR_KEY, false);
    }

    private void OnDestroy()
    {
        stoveCounter.OnProgressChanged -= Stove_OnProgressChanged;
    }

    private void Stove_OnProgressChanged(float progressAmountNormalized)
    {
        if (stoveCounter.IsFried() && progressAmountNormalized > burntShowProgressAmount)
        {
            animator.SetBool(IS_FLASHING_ANIMATOR_KEY, true);
        }
        else
        {
            animator.SetBool(IS_FLASHING_ANIMATOR_KEY, false);
        }
    }
}
