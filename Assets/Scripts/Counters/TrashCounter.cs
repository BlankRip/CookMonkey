using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
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
            KitchenObject.DestroyKitchenObject(player.GetKitchenObjectHeld());
            InteractLogicServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicServerRpc()
    {
        InteractLogicClientRpc();
    }

    [ClientRpc]
    private void InteractLogicClientRpc()
    {
        OnAnyObjectTrashed?.Invoke(this);
    }
}
