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
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    [SerializeField] RecipeSO[] allRecipiesSOArray;
    private List<RecipeSO> waitingRecipeSOList;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4.0f;
    private int waitingRecipeMax = 4;

    private void Start()
    {
        waitingRecipeSOList = new List<RecipeSO>();
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
                RecipeSO selectedRecipe = allRecipiesSOArray[Random.Range(0, allRecipiesSOArray.Length)];
                waitingRecipeSOList.Add(selectedRecipe);
                Debug.Log(selectedRecipe.RecipeName);
            }
        }
    }

    public bool DeliverRecipe(PlateKitchenObject deliveryPlate)
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
                Debug.Log("Plyaer Delivered a correct recipe");
                waitingRecipeSOList.Remove(recipe);
                return true;
            }
        }
        Debug.Log("No matches found");
        return false;
    }

}
