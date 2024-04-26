using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContatinerCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    [SerializeField] private SpriteRenderer kitchenObjectSprite;
    public Action OnPlayerGrabbedObject;

    private void Start()
    {
        kitchenObjectSprite.sprite = kitchenObjectSO.Sprite;
    }

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
