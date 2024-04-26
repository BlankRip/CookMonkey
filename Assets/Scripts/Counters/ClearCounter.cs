using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        if(!IsHoldingKitchenObject())
        {
            if(player.IsHoldingKitchenObject())
            {
                player.GetKitchenObjectHeld().SetKitchenObjectParent(this);
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
}
