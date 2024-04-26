using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [field: SerializeField]
    public KitchenObjectSO kitchenObjectSO { get; private set; }

    private IKitchenObjectParent kitchenObjectParent;

    public void SetKitchenObjectParent(IKitchenObjectParent parentKitchenObject)
    {
        if(kitchenObjectParent != null)
        {
            kitchenObjectParent.ClearKitchenbjectHeld();
        }

        kitchenObjectParent = parentKitchenObject;
        if(kitchenObjectParent.IsHoldingKitchenObject())
        {
            Debug.LogError("This IKitchenObjectParent already has a KitchenObject");
        }
        kitchenObjectParent.SetKitchenObjectHeld(this);

        transform.parent = kitchenObjectParent.GetKitchenObjectParentTransform();
        transform.localPosition = Vector3.zero;
    }

    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return kitchenObjectParent;
    }
}
