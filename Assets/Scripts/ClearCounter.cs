using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClearCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    [SerializeField] private Transform counterTopPoint;

    private KitchenObject kitchenObjectHeld;

    public void Interact(Player player)
    {
        if(kitchenObjectHeld == null)
        {
            Transform spawnedKitchenObject = GameObject.Instantiate(kitchenObjectSO.Prefab);
            spawnedKitchenObject.GetComponent<KitchenObject>().SetKitchenObjectParent(this);
        }
        else
        {
            kitchenObjectHeld.SetKitchenObjectParent(player);
        }
    }

    public Transform GetKitchenObjectParentTransform()
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

    public bool IsHoldingKitchenObject()
    {
        return kitchenObjectHeld != null;
    }
}
