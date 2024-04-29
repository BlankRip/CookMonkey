using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] StoveCounter stoveCounter;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        stoveCounter.OnStateChanged += OnStateChanged;
    }

    private void OnDestroy()
    {
        stoveCounter.OnStateChanged -= OnStateChanged;        
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
