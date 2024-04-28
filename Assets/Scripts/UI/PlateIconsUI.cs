using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateIconsUI : MonoBehaviour
{
    [SerializeField] private PlateKitchenObject plate;
    [SerializeField] private PlateIconSingleUI iconTemplate;

    private void Start()
    {
        plate.OnIngredientAdded += OnIngredientAdded;
    }

    private void OnDestroy()
    {
        plate.OnIngredientAdded -= OnIngredientAdded;
    }

    private void OnIngredientAdded(KitchenObjectSO addedKitchenObjectSO)
    {
        PlateIconSingleUI spawned = GameObject.Instantiate(iconTemplate, this.transform);
        spawned.SetKitchenObjectSprite(addedKitchenObjectSO.Sprite);
        spawned.gameObject.SetActive(true);
    }
}
