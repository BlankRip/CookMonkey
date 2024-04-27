using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] CuttingRecipeSO[] cuttingRecepiesSOArray;
    private static Dictionary<KitchenObjectSO, KitchenObjectSO> objectToSlices;

    private void Awake()
    {
        if(objectToSlices == null)
        {
            objectToSlices = new Dictionary<KitchenObjectSO, KitchenObjectSO>();
            foreach(CuttingRecipeSO cuttingRecipe in cuttingRecepiesSOArray)
            {
                objectToSlices.Add(cuttingRecipe.Input, cuttingRecipe.Output);
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!IsHoldingKitchenObject())
        {
            if (player.IsHoldingKitchenObject())
            {
                if(objectToSlices.ContainsKey(player.GetKitchenObjectHeld().KitchenObjectSO))
                {
                    player.GetKitchenObjectHeld().SetKitchenObjectParent(this);
                }
            }
        }
        else
        {
            if (!player.IsHoldingKitchenObject())
            {
                GetKitchenObjectHeld().SetKitchenObjectParent(player);
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (IsHoldingKitchenObject() && objectToSlices.ContainsKey(GetKitchenObjectHeld().KitchenObjectSO))
        {
            KitchenObjectSO slicedSO = objectToSlices[GetKitchenObjectHeld().KitchenObjectSO];
            GetKitchenObjectHeld().DestroySelf();
            KitchenObject.SpawnKitchenObject(slicedSO, this);
        }
    }
}
