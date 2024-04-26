using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounterVisual : MonoBehaviour
{
    [SerializeField] private ContatinerCounter containerCounter;
    private Animator animator;
    private const string OPEN_CLOSE_TRIGGER = "OpenClose";

    private void Start()
    {
        animator = GetComponent<Animator>();
        containerCounter.OnPlayerGrabbedObject += FireOpenCloseAnimation;
    }

    private void OnDestroy()
    {
        containerCounter.OnPlayerGrabbedObject -= FireOpenCloseAnimation;
    }

    public void FireOpenCloseAnimation()
    {
        animator.SetTrigger(OPEN_CLOSE_TRIGGER);
    }
}
