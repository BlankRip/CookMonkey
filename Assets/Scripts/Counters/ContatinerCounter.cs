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
            Transform spawnedKitchenObject = GameObject.Instantiate(kitchenObjectSO.Prefab);
            spawnedKitchenObject.GetComponent<KitchenObject>().SetKitchenObjectParent(player);
            OnPlayerGrabbedObject?.Invoke();
        }
    }
}
