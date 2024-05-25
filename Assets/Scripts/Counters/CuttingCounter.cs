using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    /// <summary>
    /// the float is the normalized progress value
    /// </summary>
    public event Action<float> OnProgressChanged;
    public event Action OnCut;
    /// <summary>
    /// Cutting Counter passed in is the sender
    /// </summary>
    public static event Action<CuttingCounter> OnAnyCut;

    [SerializeField] CuttingRecipeSO[] cuttingRecepiesSOArray;
    private static Dictionary<KitchenObjectSO, CuttingRecipeSO> cuttingRecipeDictionary;
    private int cuttingProgress;

    private void Awake()
    {
        if(cuttingRecipeDictionary == null)
        {
            cuttingRecipeDictionary = new Dictionary<KitchenObjectSO, CuttingRecipeSO>();
            foreach(CuttingRecipeSO cuttingRecipe in cuttingRecepiesSOArray)
            {
                cuttingRecipeDictionary.Add(cuttingRecipe.Input, cuttingRecipe);
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                if(cuttingRecipeDictionary.ContainsKey(player.GetKitchenObjectHeld().KitchenObjectSO))
                {
                    InteractLocgicPlaceObjectOnCounterServerRpc();
                    player.GetKitchenObjectHeld().SetKitchenObjectParent(this);
                }
            }
        }
        else
        {
            if (!player.HasKitchenObject())
            {
                GetKitchenObjectHeld().SetKitchenObjectParent(player);
                InteractLocgicPlaceObjectOnCounterServerRpc();
            }
            else
            {
                if (player.GetKitchenObjectHeld().TryGetPlate(out PlateKitchenObject plate))
                {
                    if (plate.TryAddIngredient(GetKitchenObjectHeld().KitchenObjectSO))
                    {
                        KitchenObject.DestroyKitchenObject(GetKitchenObjectHeld());
                    }
                }
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void InteractLocgicPlaceObjectOnCounterServerRpc()
    {
        InteractLocgicPlaceObjectOnCounterClientRpc();
    }

    [ClientRpc]
    public void InteractLocgicPlaceObjectOnCounterClientRpc()
    {
        cuttingProgress = 0;
        OnProgressChanged?.Invoke((float)cuttingProgress);
    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && cuttingRecipeDictionary.ContainsKey(cuttingRecipeDictionary[GetKitchenObjectHeld().KitchenObjectSO].Input))
        {
            CutObjectServerRpc();
            TestCuttingDoneServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void CutObjectServerRpc()
    {
        CutObjectClientRpc();
    }

    [ClientRpc]
    private void CutObjectClientRpc()
    {
        cuttingProgress++;
        CuttingRecipeSO currentCuttingRecipe = cuttingRecipeDictionary[GetKitchenObjectHeld().KitchenObjectSO];
        OnProgressChanged?.Invoke((float)cuttingProgress / currentCuttingRecipe.CuttingProgressMax);
        OnCut?.Invoke();
        OnAnyCut?.Invoke(this);
    }

    [ServerRpc(RequireOwnership = false)]
    private void TestCuttingDoneServerRpc()
    {
        CuttingRecipeSO currentCuttingRecipe = cuttingRecipeDictionary[GetKitchenObjectHeld().KitchenObjectSO];
        if (cuttingProgress >= currentCuttingRecipe.CuttingProgressMax)
        {
            KitchenObjectSO slicedSO = currentCuttingRecipe.Output;
            KitchenObject.DestroyKitchenObject(GetKitchenObjectHeld());
            KitchenObject.SpawnKitchenObject(slicedSO, this);
        }
    }
}
