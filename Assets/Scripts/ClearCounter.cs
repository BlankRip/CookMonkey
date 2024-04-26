using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClearCounter : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    [SerializeField] private Transform counterTopPoint;

    private KitchenObject kitchenObjectHeld;

    public void Interact()
    {
        if(kitchenObjectHeld == null)
        {
            Transform spawnedKitchenObject = GameObject.Instantiate(kitchenObjectSO.Prefab);
            spawnedKitchenObject.GetComponent<KitchenObject>().SetClearCounter(this);
        }
        else
        {
            Debug.Log("Can Pick up");
        }
    }

    public Transform GetCounterTopPointTransform()
    {
        return counterTopPoint;
    }

    public void SetKitchenObjectHeld(KitchenObject kitchenObject)
    {
        kitchenObjectHeld = kitchenObject;
    }

    public KitchenObject GetKitchenObjectHeld()
    {
        return kitchenObjectHeld;
    }

    public void ClearKitchenbjectHeld()
    {
        kitchenObjectHeld = null;
    }

    public bool HoldingAKitchenObject()
    {
        return kitchenObjectHeld != null;
    }
}
