using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisuals : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    [SerializeField] private GameObject stoveOn;
    [SerializeField] private GameObject particles;

    private void Start()
    {
        stoveCounter.OnStateChanged += OnStateChanged;
    }

    private void OnDestroy()
    {
        stoveCounter.OnStateChanged -= OnStateChanged;
    }

    private void OnStateChanged(StoveCounter.State state)
    {
        bool showFlameOn = state != StoveCounter.State.Idle;
        bool showParticles = state == StoveCounter.State.Frying || state == StoveCounter.State.Fried;
        stoveOn.SetActive(showFlameOn);
        particles.SetActive(showParticles);
    }
}
