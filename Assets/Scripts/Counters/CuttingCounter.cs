using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] CuttingRecipeSO[] cuttingRecepiesSOArray;

    public override void Interact(Player player)
    {
        if (!IsHoldingKitchenObject())
        {
            if (player.IsHoldingKitchenObject())
            {
                if(true /*Change to has reciepy check in a better way*/)
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
        if (IsHoldingKitchenObject())
        {
            KitchenObjectSO slicedSO = GetOutPutForInput(GetKitchenObjectHeld().KitchenObjectSO);
            if(slicedSO != null)
            {
                GetKitchenObjectHeld().DestroySelf();
                KitchenObject.SpawnKitchenObject(slicedSO, this);
            }
        }
    }

    private KitchenObjectSO GetOutPutForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipe in cuttingRecepiesSOArray)
        {
            if(cuttingRecipe.Input == inputKitchenObjectSO)
            {
                return cuttingRecipe.Output;
            }
        }
        return null;
    }
}
