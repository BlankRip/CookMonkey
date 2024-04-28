using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKitchenObjectParent
{
    public Transform GetKitchenObjectParentTransform();
    public void SetKitchenObjectHeld(KitchenObject kitchenObject);
    public KitchenObject GetKitchenObjectHeld();
    public void ClearKitchenbjectHeld();
    public bool HasKitchenObject();
}
