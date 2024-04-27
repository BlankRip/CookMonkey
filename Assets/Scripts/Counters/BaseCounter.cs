using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] private Transform counterTopPoint;

    private KitchenObject kitchenObjectHeld;

    public virtual void Interact(Player player)
    {
        Debug.Log("Base Class has no intreact logic");
    }

    public virtual void InteractAlternate(Player player)
    {
    }

    #region IKitchenObjectParent
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

    public bool HasKitchenObject()
    {
        return kitchenObjectHeld != null;
    }
    #endregion
}
