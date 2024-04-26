using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [field: SerializeField]
    public KitchenObjectSO kitchenObjectSO { get; private set; }

    private ClearCounter clearCounter;

    public void SetClearCounter(ClearCounter counterItIsPlacedOn)
    {
        if(clearCounter != null)
        {
            clearCounter.ClearKitchenbjectHeld();
        }

        clearCounter = counterItIsPlacedOn;
        if(clearCounter.HoldingAKitchenObject())
        {
            Debug.LogError("This counter already has a KitchenObject", clearCounter.gameObject);
        }
        clearCounter.SetKitchenObjectHeld(this);

        transform.parent = clearCounter.GetCounterTopPointTransform();
        transform.localPosition = Vector3.zero;
    }

    public ClearCounter GetClearCounter()
    {
        return clearCounter;
    }
}
