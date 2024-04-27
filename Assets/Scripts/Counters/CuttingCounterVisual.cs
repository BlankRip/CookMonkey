using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour
{
    [SerializeField] private CuttingCounter cuttingCounter;
    private Animator animator;
    private const string CUT = "Cut";

    private void Start()
    {
        animator = GetComponent<Animator>();
        cuttingCounter.OnCut += FireCutAnimation;
    }

    private void OnDestroy()
    {
        cuttingCounter.OnCut -= FireCutAnimation;
    }

    public void FireCutAnimation()
    {
        animator.SetTrigger(CUT);
    }
}
