using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
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
        spawnRecipeTimer = spawnRecipeTimerMax;
    }

    private void Update()
    {
        if(waitingRecipeSOList.Count < waitingRecipeMax)
        {
            spawnRecipeTimer += Time.deltaTime;
            if(spawnRecipeTimer >= spawnRecipeTimerMax)
            {
                spawnRecipeTimer = 0;
                RecipeSO selectedRecipe = allRecipiesSOArray[UnityEngine.Random.Range(0, allRecipiesSOArray.Length)];
                waitingRecipeSOList.Add(selectedRecipe);
                OnRecipeSpawn?.Invoke();
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject deliveryPlate)
    {
        List<KitchenObjectSO> kitchenObjectsOnPlate = deliveryPlate.GetItemsOnPlateKitchenObectSOList();
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
                waitingRecipeSOList.Remove(recipe);
                OnRecipeCompleted?.Invoke();
                successfulDeliveriesAmount++;
                return;
            }
        }
        OnRecipeFailed?.Invoke();
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
