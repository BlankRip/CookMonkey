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
            
        }
        else
        {
            GetKitchenObjectHeld().SetKitchenObjectParent(player);
        }
    }
}
