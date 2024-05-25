using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        if(!HasKitchenObject())
        {
            if(player.HasKitchenObject())
            {
                player.GetKitchenObjectHeld().SetKitchenObjectParent(this);
            }
        }
        else
        {
            if (!player.HasKitchenObject())
            {
                GetKitchenObjectHeld().SetKitchenObjectParent(player);
            }
            else
            {
                if (player.GetKitchenObjectHeld().TryGetPlate(out PlateKitchenObject plateOnPlayer))
                {
                    if (plateOnPlayer.TryAddIngredient(GetKitchenObjectHeld().KitchenObjectSO))
                    {
                        KitchenObject.DestroyKitchenObject(GetKitchenObjectHeld());
                    }
                }
                else if (GetKitchenObjectHeld().TryGetPlate(out PlateKitchenObject plateOnCounter))
                {
                    if (plateOnCounter.TryAddIngredient(player.GetKitchenObjectHeld().KitchenObjectSO))
                    {
                        KitchenObject.DestroyKitchenObject(player.GetKitchenObjectHeld());
                    }
                }
                
            }
        }
    }
}
