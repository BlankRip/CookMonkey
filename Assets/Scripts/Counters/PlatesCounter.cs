using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event Action OnPlateSpawned;
    public event Action OnPlateRemoved;

    [SerializeField] KitchenObjectSO plateKitchenObjectSO;
    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4.0f;
    private int plateSpawnAmount;
    private int plateSpawnAmountMax = 4;

    private void Update()
    {
        if(!IsServer)
            return;

        if (KitchenGameManager.Instance.IsGamePlaying() && plateSpawnAmount < plateSpawnAmountMax)
        {
            spawnPlateTimer += Time.deltaTime;
            if (spawnPlateTimer > spawnPlateTimerMax)
            {
                spawnPlateTimer = 0;
                SpawnPlateClientRpc();
            }
        }
    }

    [ClientRpc]
    private void SpawnPlateClientRpc()
    {
        plateSpawnAmount++;
        OnPlateSpawned?.Invoke();
    }

    public override void Interact(Player player)
    {
        if(plateSpawnAmount > 0 && !player.HasKitchenObject())
        {
            KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
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
        plateSpawnAmount--;
        OnPlateRemoved?.Invoke();
    }
}