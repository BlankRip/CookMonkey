using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContatinerCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    public Action OnPlayerGrabbedObject;

    public override void Interact(Player player)
    {
        if(!player.IsHoldingKitchenObject())
        {
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);
            OnPlayerGrabbedObject?.Invoke();
        }
    }
}
