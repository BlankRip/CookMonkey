using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class KitchenGameMultiplayer : NetworkBehaviour
{
    public static KitchenGameMultiplayer Instance { get; private set; }
    [SerializeField] private KitchenObjectSOList allKitchenObjects;
    private Dictionary<KitchenObjectSO, int> allKitchenObjectsIndexDictionary;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            allKitchenObjectsIndexDictionary = new Dictionary<KitchenObjectSO, int>();
            for (int i = 0; i < allKitchenObjects.KitchenObjects.Count; i++)
            {
                allKitchenObjectsIndexDictionary.Add(allKitchenObjects.KitchenObjects[i], i);
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void StartHost()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += NetworkManager_ConnectionApprovalCallBack;
        NetworkManager.Singleton.StartHost();
    }

    private void NetworkManager_ConnectionApprovalCallBack(NetworkManager.ConnectionApprovalRequest connectionApprovalRequest, 
        NetworkManager.ConnectionApprovalResponse connectionApprovalResponse)
    {
        if(KitchenGameManager.Instance.IsWaitingToStart())
        {
            connectionApprovalResponse.Approved = true;
            connectionApprovalResponse.CreatePlayerObject = true;
        }
        else
        {
            connectionApprovalResponse.Approved = false;
        }
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }

    public int GetKitchonObjectSOIndex(KitchenObjectSO kitchenObjectSO)
    {
        return allKitchenObjectsIndexDictionary[kitchenObjectSO];
    }

    public KitchenObjectSO GetKitchenObjectSOFromIndex(int index)
    {
        return allKitchenObjects.KitchenObjects[index];
    }

    public void SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    {
        SpawnKitchenObjectServerRpc(GetKitchonObjectSOIndex(kitchenObjectSO), kitchenObjectParent.GetNetworkObject());
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnKitchenObjectServerRpc(int kitchenObjectSOIndex, NetworkObjectReference kitchenObjectParentNetworkObjectReference)
    {
        Transform spawnedKitchenObject = GameObject.Instantiate(GetKitchenObjectSOFromIndex(kitchenObjectSOIndex).Prefab);
        NetworkObject kitchenNetworkObject = spawnedKitchenObject.GetComponent<NetworkObject>();
        kitchenNetworkObject.Spawn(true);

        KitchenObject kitchenObj = spawnedKitchenObject.GetComponent<KitchenObject>();
        kitchenObjectParentNetworkObjectReference.TryGet(out NetworkObject kitchenObjectParentNetworkObject);
        IKitchenObjectParent kitchenObjectParent = kitchenObjectParentNetworkObject.GetComponent<IKitchenObjectParent>();
        kitchenObj.SetKitchenObjectParent(kitchenObjectParent);
    }

    public void DestroyKitchenObject(KitchenObject kitchenObject)
    {
        DestroyKitchenObjectServerRpc(kitchenObject.NetworkObject);
    }

    [ServerRpc(RequireOwnership = false)]
    private void DestroyKitchenObjectServerRpc(NetworkObjectReference kitchenObjectNetworkObjectReference)
    {
        kitchenObjectNetworkObjectReference.TryGet(out NetworkObject kitchenObjectNetworkObject);
        KitchenObject kitchenObject = kitchenObjectNetworkObject.GetComponent<KitchenObject>();

        ClearKitchenObjectOnParentClientRpc(kitchenObjectNetworkObjectReference);
        kitchenObject.DestroySelf();
    }

    [ClientRpc]
    private void ClearKitchenObjectOnParentClientRpc(NetworkObjectReference kitchenObjectNetworkObjectReference)
    {
        kitchenObjectNetworkObjectReference.TryGet(out NetworkObject kitchenObjectNetworkObject);
        KitchenObject kitchenObject = kitchenObjectNetworkObject.GetComponent<KitchenObject>();

        kitchenObject.ClearKitchenObjectOnParent();
    }
}
