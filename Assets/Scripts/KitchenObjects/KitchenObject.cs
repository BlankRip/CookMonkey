using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class KitchenObject : NetworkBehaviour
{
    [field: SerializeField]
    public KitchenObjectSO KitchenObjectSO { get; private set; }

    private IKitchenObjectParent kitchenObjectParent;
    private FollowTransform followTransform;

    protected virtual void Start()
    {
        
    }

    public void SetKitchenObjectParent(IKitchenObjectParent parentKitchenObject)
    {
        SetKitchenObjectParentServerRpc(parentKitchenObject.GetNetworkObject());
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetKitchenObjectParentServerRpc(NetworkObjectReference kitchenObjectParentNetworkObjectReference)
    {
        SetKitchenObjectParentClientRpc(kitchenObjectParentNetworkObjectReference);
    }

    [ClientRpc]
    private void SetKitchenObjectParentClientRpc(NetworkObjectReference kitchenObjectParentNetworkObjectReference)
    {
        kitchenObjectParentNetworkObjectReference.TryGet(out NetworkObject kitchenObjectParentNetworkObject);
        IKitchenObjectParent parentKitchenObject = kitchenObjectParentNetworkObject.GetComponent<IKitchenObjectParent>();

        if (kitchenObjectParent != null)
        {
            kitchenObjectParent.ClearKitchenbjectHeld();
        }

        kitchenObjectParent = parentKitchenObject;
        if (kitchenObjectParent.HasKitchenObject())
        {
            Debug.LogError("This IKitchenObjectParent already has a KitchenObject");
        }
        kitchenObjectParent.SetKitchenObjectHeld(this);

        if(followTransform == null)
        {
            followTransform = GetComponent<FollowTransform>();
        }
        followTransform.SetTargetTransform(kitchenObjectParent.GetKitchenObjectParentTransform());
    }



    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return kitchenObjectParent;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void ClearKitchenObjectOnParent()
    {
        kitchenObjectParent.ClearKitchenbjectHeld();
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

    public static void SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    {
        KitchenGameMultiplayer.Instance.SpawnKitchenObject(kitchenObjectSO, kitchenObjectParent);
    }

    public static void DestroyKitchenObject(KitchenObject kitchenObject)
    {
        KitchenGameMultiplayer.Instance.DestroyKitchenObject(kitchenObject);
    }
}
