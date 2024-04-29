using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    /// <summary>
    /// the float is the normalized progress value
    /// </summary>
    public event Action<float> OnProgressChanged;
    public event Action OnCut;
    /// <summary>
    /// Cutting Counter passed in is the sender
    /// </summary>
    public static event Action<CuttingCounter> OnAnyCut;

    [SerializeField] CuttingRecipeSO[] cuttingRecepiesSOArray;
    private static Dictionary<KitchenObjectSO, CuttingRecipeSO> cuttingRecipeDictionary;
    CuttingRecipeSO currentCuttingRecipe;
    private int cuttingProgress;

    private void Awake()
    {
        if(cuttingRecipeDictionary == null)
        {
            cuttingRecipeDictionary = new Dictionary<KitchenObjectSO, CuttingRecipeSO>();
            foreach(CuttingRecipeSO cuttingRecipe in cuttingRecepiesSOArray)
            {
                cuttingRecipeDictionary.Add(cuttingRecipe.Input, cuttingRecipe);
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                if(cuttingRecipeDictionary.ContainsKey(player.GetKitchenObjectHeld().KitchenObjectSO))
                {
                    cuttingProgress = 0;
                    OnProgressChanged?.Invoke((float)cuttingProgress);
                    currentCuttingRecipe = cuttingRecipeDictionary[player.GetKitchenObjectHeld().KitchenObjectSO];
                    player.GetKitchenObjectHeld().SetKitchenObjectParent(this);
                }
            }
        }
        else
        {
            if (!player.HasKitchenObject())
            {
                GetKitchenObjectHeld().SetKitchenObjectParent(player);
                currentCuttingRecipe = null;
                OnProgressChanged?.Invoke(0);
            }
            else
            {
                if (player.GetKitchenObjectHeld().TryGetPlate(out PlateKitchenObject plate))
                {
                    if (plate.TryAddIngredient(GetKitchenObjectHeld().KitchenObjectSO))
                    {
                        GetKitchenObjectHeld().DestroySelf();
                    }
                }
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && currentCuttingRecipe.Input == GetKitchenObjectHeld().KitchenObjectSO)
        {
            cuttingProgress++;
            OnProgressChanged?.Invoke((float)cuttingProgress/ currentCuttingRecipe.CuttingProgressMax);
            OnCut?.Invoke();
            OnAnyCut?.Invoke(this);
            if (cuttingProgress >= currentCuttingRecipe.CuttingProgressMax)
            {
                KitchenObjectSO slicedSO = currentCuttingRecipe.Output;
                GetKitchenObjectHeld().DestroySelf();
                KitchenObject.SpawnKitchenObject(slicedSO, this);
            }
        }
    }
}
