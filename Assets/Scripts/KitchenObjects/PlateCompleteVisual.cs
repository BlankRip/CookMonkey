using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [System.Serializable]
    private struct KitchenObjectSO_GameObject
    {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject gameObject;
    }

    [SerializeField] private PlateKitchenObject plate;
    [SerializeField] private List<KitchenObjectSO_GameObject> kitchenObjectSOGameObjectList;

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
        foreach (KitchenObjectSO_GameObject kitchenObjectSOGameObject in kitchenObjectSOGameObjectList)
        {
            if(kitchenObjectSOGameObject.kitchenObjectSO == addedKitchenObjectSO)
            {
                kitchenObjectSOGameObject.gameObject.SetActive(true);
                break;
            }
        }
    }
}
