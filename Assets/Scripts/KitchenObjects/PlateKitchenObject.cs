using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    /// <summary>
    /// KitchenObjectSO is the ingredient that is added to the plate
    /// </summary>
    public Action<KitchenObjectSO> OnIngredientAdded;

    [SerializeField] List<KitchenObjectSO> validKitchenObjectSOList;
    private List<KitchenObjectSO> kitchenObjectSOList;

    private void Start()
    {
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if (kitchenObjectSOList.Contains(kitchenObjectSO) || !validKitchenObjectSOList.Contains(kitchenObjectSO))
        {
            return false;
        }
        AddIngredientServerRpc(KitchenGameMultiplayer.Instance.GetKitchonObjectSOIndex(kitchenObjectSO));
        return true;
    }

    [ServerRpc(RequireOwnership = false)]
    private void AddIngredientServerRpc(int kitchenObjectSOIndex)
    {
        AddIngredientClientRpc(kitchenObjectSOIndex);
    }

    [ClientRpc]
    private void AddIngredientClientRpc(int kitchenObjectSOIndex)
    {
        KitchenObjectSO kitchenObjectSO = KitchenGameMultiplayer.Instance.GetKitchenObjectSOFromIndex(kitchenObjectSOIndex);
        kitchenObjectSOList.Add(kitchenObjectSO);
        OnIngredientAdded?.Invoke(kitchenObjectSO);
    }

    public List<KitchenObjectSO> GetItemsOnPlateKitchenObectSOList()
    {
        return kitchenObjectSOList;
    }
}
