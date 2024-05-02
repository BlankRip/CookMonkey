using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] StoveCounter stoveCounter;
    private AudioSource audioSource;
    private float burntShowProgressAmount = 0.5f;
    private float warningSignTimer;
    private float warnginSignTimerMax = 0.2f;
    private bool playWarningSound;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        stoveCounter.OnStateChanged += OnStateChanged;
        stoveCounter.OnProgressChanged += Stove_OnProgressChanged;
    }

    private void OnDestroy()
    {
        stoveCounter.OnStateChanged -= OnStateChanged;
        stoveCounter.OnProgressChanged -= Stove_OnProgressChanged;
    }

    private void Update()
    {
        if(playWarningSound)
        {
            warningSignTimer -= Time.deltaTime;
            if(warningSignTimer < 0)
            {
                warningSignTimer = warnginSignTimerMax;
                SoundManager.Instance.PlayeWarningSound(stoveCounter.transform.position);
            }
        }
    }

    private void Stove_OnProgressChanged(float progressAmountNormalized)
    {
        if (stoveCounter.IsFried() && progressAmountNormalized > burntShowProgressAmount)
        {
            playWarningSound = true;
        }
        else
        {
            playWarningSound = false;
        }
    }

    private void OnStateChanged(StoveCounter.State state)
    {
        bool playSound = state == StoveCounter.State.Frying || state == StoveCounter.State.Fried;
        if (playSound)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Pause();
        }
    }
}
