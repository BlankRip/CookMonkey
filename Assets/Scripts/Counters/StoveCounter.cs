using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    public enum State { Idle, Frying, Fried, Burnt }

    public event Action<float> OnProgressChanged;
    public event Action<State> OnStateChanged;

    [SerializeField] FryingRecipeSO[] fryingRecipeSOArray;
    private static Dictionary<KitchenObjectSO, FryingRecipeSO> fryingRecipeDictionary;
    private State currentState;
    private float fryingTimer;
    private float burningTimer;
    FryingRecipeSO currentFryingRecipe;

    private void Awake()
    {
        if (fryingRecipeDictionary == null)
        {
            fryingRecipeDictionary = new Dictionary<KitchenObjectSO, FryingRecipeSO>();
            foreach (FryingRecipeSO fryingRecipe in fryingRecipeSOArray)
            {
                fryingRecipeDictionary.Add(fryingRecipe.Input, fryingRecipe);
            }
        }
    }

    private void Start()
    {
        SetState(State.Idle);
    }

    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (currentState)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    FryingState();
                    break;
                case State.Fried:
                    FriedState();
                    break;
                case State.Burnt:
                    break;
            }
        }
    }

    private void FryingState()
    {
        fryingTimer += Time.deltaTime;
        OnProgressChanged?.Invoke((float)fryingTimer / currentFryingRecipe.FryingTimerMax);
        if (fryingTimer >= currentFryingRecipe.FryingTimerMax)
        {
            KitchenObjectSO friedSO = currentFryingRecipe.Output;
            GetKitchenObjectHeld().DestroySelf();
            KitchenObject.SpawnKitchenObject(friedSO, this);

            if (fryingRecipeDictionary.ContainsKey(currentFryingRecipe.Output))
            {
                SetState(State.Fried);
                currentFryingRecipe = fryingRecipeDictionary[currentFryingRecipe.Output];
                burningTimer = 0;
            }
            else
            {
                Debug.LogError("there is no recepy to move from fried to burnt");
                SetState(State.Idle);
            }
        }
    }

    private void FriedState()
    {
        burningTimer += Time.deltaTime;
        OnProgressChanged?.Invoke((float)burningTimer / currentFryingRecipe.FryingTimerMax);
        if (burningTimer >= currentFryingRecipe.FryingTimerMax)
        {
            KitchenObjectSO friedSO = currentFryingRecipe.Output;
            GetKitchenObjectHeld().DestroySelf();
            KitchenObject.SpawnKitchenObject(friedSO, this);
            SetState(State.Burnt);
        }
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                if (fryingRecipeDictionary.ContainsKey(player.GetKitchenObjectHeld().KitchenObjectSO))
                {
                    fryingTimer = 0;
                    OnProgressChanged?.Invoke((float)fryingTimer);
                    currentFryingRecipe = fryingRecipeDictionary[player.GetKitchenObjectHeld().KitchenObjectSO];
                    player.GetKitchenObjectHeld().SetKitchenObjectParent(this);
                    SetState(State.Frying);
                }
            }
        }
        else
        {
            if (!player.HasKitchenObject())
            {
                GetKitchenObjectHeld().SetKitchenObjectParent(player);
                currentFryingRecipe = null;
                SetState(State.Idle);
                OnProgressChanged?.Invoke(0);
            }
            else
            {
                if (player.GetKitchenObjectHeld().TryGetPlate(out PlateKitchenObject plate))
                {
                    if (plate.TryAddIngredient(GetKitchenObjectHeld().KitchenObjectSO))
                    {
                        GetKitchenObjectHeld().DestroySelf();
                        currentFryingRecipe = null;
                        SetState(State.Idle);
                        OnProgressChanged?.Invoke(0);
                    }
                }
            }
        }
    }

    private void SetState(State state)
    {
        currentState = state;
        OnStateChanged?.Invoke(currentState);
    }
}