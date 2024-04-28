using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [field: SerializeField]
    public KitchenObjectSO KitchenObjectSO { get; private set; }

    private IKitchenObjectParent kitchenObjectParent;

    public void SetKitchenObjectParent(IKitchenObjectParent parentKitchenObject)
    {
        if(kitchenObjectParent != null)
        {
            kitchenObjectParent.ClearKitchenbjectHeld();
        }

        kitchenObjectParent = parentKitchenObject;
        if(kitchenObjectParent.HasKitchenObject())
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

    public void DestroySelf()
    {
        kitchenObjectParent.ClearKitchenbjectHeld();
        Destroy(gameObject);
    }

    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        if(this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }

        plateKitchenObject = null;
        return false;
    }

    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    {
        Transform spawnedKitchenObject = GameObject.Instantiate(kitchenObjectSO.Prefab);
        KitchenObject kitchenObj = spawnedKitchenObject.GetComponent<KitchenObject>();
        kitchenObj.SetKitchenObjectParent(kitchenObjectParent);

        return kitchenObj;
    }
}
