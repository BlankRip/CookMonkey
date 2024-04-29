using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            if(player.GetKitchenObjectHeld().TryGetPlate(out PlateKitchenObject plate))
            {
                DeliveryManager.Instance.DeliverRecipe(plate);
                player.GetKitchenObjectHeld().DestroySelf();
            }
        }
    }
}
