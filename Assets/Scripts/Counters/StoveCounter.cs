using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    public enum State { Idle, Frying, Fried, Burnt }

    public event Action<float> OnProgressChanged;
    public event Action<State> OnStateChanged;

    [SerializeField] FryingRecipeSO[] fryingRecipeSOArray;
    private static Dictionary<KitchenObjectSO, FryingRecipeSO> fryingRecipeDictionary;
    private NetworkVariable<State> currentState = new NetworkVariable<State>(State.Idle);
    private NetworkVariable<float> fryingTimer = new NetworkVariable<float>(0f);
    private NetworkVariable<float> burningTimer = new NetworkVariable<float>(0f);
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

    public override void OnNetworkSpawn()
    {
        fryingTimer.OnValueChanged += FryingTimerOnValueChanged;
        burningTimer.OnValueChanged += BurningTimerOnValueChanged;
        currentState.OnValueChanged += CurrentStateOnValueChanged;
    }

    public override void OnNetworkDespawn()
    {
        fryingTimer.OnValueChanged -= FryingTimerOnValueChanged;
        burningTimer.OnValueChanged -= BurningTimerOnValueChanged;
        currentState.OnValueChanged -= CurrentStateOnValueChanged;
    }

    private void Update()
    {
        if (!IsServer)
            return;

        if (HasKitchenObject())
        {
            switch (currentState.Value)
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
        fryingTimer.Value += Time.deltaTime;
        
        if (fryingTimer.Value >= currentFryingRecipe.FryingTimerMax)
        {
            KitchenObjectSO friedSO = currentFryingRecipe.Output;
            KitchenObject.DestroyKitchenObject(GetKitchenObjectHeld());
            KitchenObject.SpawnKitchenObject(friedSO, this);

            if (fryingRecipeDictionary.ContainsKey(currentFryingRecipe.Output))
            {
                SetState(State.Fried);
                SetFryingRecipeSOClientRpc(KitchenGameMultiplayer.Instance.GetKitchonObjectSOIndex(currentFryingRecipe.Output));
                burningTimer.Value = 0;
            }
            else
            {
                SetState(State.Burnt);
            }
        }
    }

    private void FryingTimerOnValueChanged(float previousValue, float newValue)
    {
        float timerMax = currentFryingRecipe != null ? currentFryingRecipe.FryingTimerMax : 1.0f;
        OnProgressChanged?.Invoke((float)fryingTimer.Value / timerMax);
    }

    private void FriedState()
    {
        burningTimer.Value += Time.deltaTime;
        OnProgressChanged?.Invoke((float)burningTimer.Value / currentFryingRecipe.FryingTimerMax);
        if (burningTimer.Value >= currentFryingRecipe.FryingTimerMax)
        {
            KitchenObjectSO friedSO = currentFryingRecipe.Output;
            KitchenObject.DestroyKitchenObject(GetKitchenObjectHeld());
            KitchenObject.SpawnKitchenObject(friedSO, this);
            SetState(State.Burnt);
        }
    }

    private void BurningTimerOnValueChanged(float previousValue, float newValue)
    {
        float timerMax = currentFryingRecipe != null ? currentFryingRecipe.FryingTimerMax : 1.0f;
        OnProgressChanged?.Invoke((float)burningTimer.Value / timerMax);
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                if (fryingRecipeDictionary.ContainsKey(player.GetKitchenObjectHeld().KitchenObjectSO))
                {
                    KitchenObject kitchenObject = player.GetKitchenObjectHeld();
                    player.GetKitchenObjectHeld().SetKitchenObjectParent(this);
                    InteractLocgicPlaceObjectOnCounterServerRpc(KitchenGameMultiplayer.Instance.GetKitchonObjectSOIndex(
                        kitchenObject.KitchenObjectSO));
                }
            }
        }
        else
        {
            if (!player.HasKitchenObject())
            {
                GetKitchenObjectHeld().SetKitchenObjectParent(player);
                SetIdleStateServerRpc();
            }
            else
            {
                if (player.GetKitchenObjectHeld().TryGetPlate(out PlateKitchenObject plate))
                {
                    if (plate.TryAddIngredient(GetKitchenObjectHeld().KitchenObjectSO))
                    {
                        KitchenObject.DestroyKitchenObject(GetKitchenObjectHeld());
                        SetIdleStateServerRpc();
                    }
                }
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void InteractLocgicPlaceObjectOnCounterServerRpc(int kitchenObjectSOIndex)
    {
        fryingTimer.Value = 0;
        SetState(State.Frying);
        SetFryingRecipeSOClientRpc(kitchenObjectSOIndex);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetIdleStateServerRpc()
    {
        SetState(State.Idle);
    }

    [ClientRpc]
    public void SetFryingRecipeSOClientRpc(int kitchenObjectSOIndex)
    {
        currentFryingRecipe = fryingRecipeDictionary[KitchenGameMultiplayer.Instance.GetKitchenObjectSOFromIndex(kitchenObjectSOIndex)];
    }

    private void SetState(State state)
    {
        currentState.Value = state;
        OnStateChanged?.Invoke(currentState.Value);
    }

    private void CurrentStateOnValueChanged(State previousValue, State newValue)
    {
        if(newValue != previousValue)
        {
            if(newValue == State.Idle || newValue == State.Burnt)
            {
                OnProgressChanged?.Invoke(0.0f);
            }
        }
        OnStateChanged?.Invoke(currentState.Value);
    }

    public bool IsFried()
    {
        return currentState.Value == State.Fried;
    }
}