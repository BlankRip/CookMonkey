using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeSingleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipeNameText;
    [SerializeField] private Transform iconContainer;
    [SerializeField] private Image iconTemplate;

    public void SetRecipe(RecipeSO recipe)
    {
        recipeNameText.SetText(recipe.RecipeName);
        List<KitchenObjectSO> recipeIngredients = recipe.kitchenObjectSOList;
        foreach (KitchenObjectSO kitchenObject in recipeIngredients)
        {
            Image spawnedImage = GameObject.Instantiate(iconTemplate, iconContainer);
            spawnedImage.sprite = kitchenObject.Sprite;
            spawnedImage.gameObject.SetActive(true);
        }
    }
}
