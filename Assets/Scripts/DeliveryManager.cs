using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.Netcode;
using UnityEngine;

public class DeliveryManager : NetworkBehaviour
{
    #region Singleton
    public static DeliveryManager Instance;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            waitingRecipeSOList = new List<RecipeSO>();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    public Action OnRecipeSpawn;
    public Action OnRecipeCompleted;
    public Action OnRecipeFailed;

    [SerializeField] RecipeSO[] allRecipiesSOArray;
    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4.0f;
    private int waitingRecipeMax = 4;
    private int successfulDeliveriesAmount;

    private void Start()
    {
        //spawnRecipeTimer = spawnRecipeTimerMax;
        spawnRecipeTimer = 0;
    }

    private void Update()
    {
        if (!IsServer)
            return;

        if(KitchenGameManager.Instance.IsGamePlaying() && waitingRecipeSOList.Count < waitingRecipeMax)
        {
            spawnRecipeTimer += Time.deltaTime;
            if(spawnRecipeTimer >= spawnRecipeTimerMax)
            {
                spawnRecipeTimer = 0;
                int selectedRecipe = UnityEngine.Random.Range(0, allRecipiesSOArray.Length);
                SpawnNewWaitingRecipeClientRpc(selectedRecipe);
            }
        }
    }

    [ClientRpc]
    private void SpawnNewWaitingRecipeClientRpc(int waitingRecipeSOIndex)
    {
        waitingRecipeSOList.Add(allRecipiesSOArray[waitingRecipeSOIndex]);
        OnRecipeSpawn?.Invoke();
    }

    public void DeliverRecipe(PlateKitchenObject deliveryPlate)
    {
        List<KitchenObjectSO> kitchenObjectsOnPlate = deliveryPlate.GetItemsOnPlateKitchenObectSOList();
        int index = 0;
        foreach (RecipeSO recipe in waitingRecipeSOList)
        {
            bool plateContentMatchedRecipe = false;
            if(recipe.kitchenObjectSOList.Count == kitchenObjectsOnPlate.Count)
            {
                plateContentMatchedRecipe = true;
                foreach (KitchenObjectSO kitchenObject in kitchenObjectsOnPlate)
                {
                    if(!recipe.kitchenObjectSOList.Contains(kitchenObject))
                    {
                        plateContentMatchedRecipe = false;
                        break;
                    }
                }
            }
            if (plateContentMatchedRecipe)
            {
                DeliverRecipeServerRpc(index, true);
                index++;
                return;
            }
            index++;
        }
        DeliverRecipeServerRpc(-1, false);
    }

    [ServerRpc(RequireOwnership = false)]
    private void DeliverRecipeServerRpc(int waitingRecipeListSOIndex, bool success)
    {
        DeliverRecipeClientRpc(waitingRecipeListSOIndex, success);
    }

    [ClientRpc]
    private void DeliverRecipeClientRpc(int waitingRecipeListSOIndex, bool success)
    {
        if(success)
        {
            waitingRecipeSOList.Remove(waitingRecipeSOList[waitingRecipeListSOIndex]);
            OnRecipeCompleted?.Invoke();
            successfulDeliveriesAmount++;
        }
        else
        {
            OnRecipeFailed?.Invoke();
        }
    }

    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeSOList;
    }

    public int GetSuccessfulDeliveriesAmount()
    {
        return successfulDeliveriesAmount;
    }
}
