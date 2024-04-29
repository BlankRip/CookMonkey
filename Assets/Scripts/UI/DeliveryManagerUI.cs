using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] private Transform contatiner;
    [SerializeField] private RecipeSingleUI recipeTemplate;


    private void Start()
    {
        UpdateVisual();
        DeliveryManager.Instance.OnRecipeSpawn += UpdateVisual;
        DeliveryManager.Instance.OnRecipeCompleted += UpdateVisual;
    }

    private void OnDestroy()
    {
        DeliveryManager.Instance.OnRecipeSpawn -= UpdateVisual;
        DeliveryManager.Instance.OnRecipeCompleted -= UpdateVisual;
    }

    private void UpdateVisual()
    {
        for (int i = 1; i < contatiner.childCount; i++)
        {
            Destroy(contatiner.GetChild(i).gameObject);
        }

        List<RecipeSO> waitingRecipes = DeliveryManager.Instance.GetWaitingRecipeSOList();
        foreach (RecipeSO recipe in waitingRecipes)
        {
            RecipeSingleUI spawnedItem = GameObject.Instantiate(recipeTemplate, contatiner);
            spawnedItem.SetRecipe(recipe);
            spawnedItem.transform.SetAsLastSibling();
            spawnedItem.gameObject.SetActive(true);
        }
    }
}
