using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    /// <summary>
    /// Trash Counter passed in is the sender
    /// </summary>
    public static event Action<TrashCounter> OnAnyObjectTrashed;

    public override void Interact(Player player)
    {
        if(player.HasKitchenObject())
        {
            player.GetKitchenObjectHeld().DestroySelf();
            OnAnyObjectTrashed?.Invoke(this);
        }
    }
}
