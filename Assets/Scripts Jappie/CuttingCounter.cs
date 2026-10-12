using System;
using UnityEngine;

public class CuttingCounter : InteractableCC, IHasProgress
{
    [SerializeField] private Transform cuttingPoint;

    public event Action<float> OnProgressChanged;    // 0 tot 1
    public event Action<bool> OnFoodPresenceChanged; // true = balk tonen

    private FoodObjects currentFood;
    private int cuttingProgress;
    private bool isSliced;

    // E: neerleggen of terugpakken
    public override bool CanInteract(PlayerMovement player)
    {
        if (player.HeldItem != null)
            return currentFood == null && player.HeldItem is FoodObjects;

        return currentFood != null;
    }

    public override void Interact(PlayerMovement player)
    {
        if (player.HeldItem != null)
            PlaceFood((FoodObjects)player.Release());
        else
            player.PickUp(TakeFood());
    }

    // F: snijden
    public override bool CanInteractAlternate(PlayerMovement player)
    {
        return currentFood != null && !isSliced && CanBeSliced(currentFood);
    }

    public override void InteractAlternate(PlayerMovement player)
    {
        SliceFood();
    }

    private void PlaceFood(FoodObjects food)
    {
        currentFood = food;
        isSliced = false;
        food.transform.SetParent(cuttingPoint);
        food.transform.localPosition = Vector3.zero;
        food.transform.localRotation = Quaternion.identity;

        ResetProgress();
        OnFoodPresenceChanged?.Invoke(CanBeSliced(food));
    }

    private FoodObjects TakeFood()
    {
        FoodObjects food = currentFood;
        currentFood = null;
        isSliced = false;
        ResetProgress();
        OnFoodPresenceChanged?.Invoke(false);
        return food;
    }

    private bool CanBeSliced(FoodObjects food)
    {
        return food.GetKitchenObjectSO().slicedVersion != null;
    }

    private void SliceFood()
    {
        FoodSO currentSO = currentFood.GetKitchenObjectSO();

        cuttingProgress++;
        OnProgressChanged?.Invoke((float)cuttingProgress / currentSO.cuttingProgressMax);

        if (cuttingProgress >= currentSO.cuttingProgressMax)
        {
            FoodSO slicedSO = currentSO.slicedVersion;
            currentFood.DestroySelf();

            Transform spawned = Instantiate(slicedSO.prefab, cuttingPoint);
            spawned.localPosition = Vector3.zero;
            currentFood = spawned.GetComponent<FoodObjects>();

            isSliced = true;
            ResetProgress();
            OnFoodPresenceChanged?.Invoke(false);
        }
    }

    private void ResetProgress()
    {
        cuttingProgress = 0;
        OnProgressChanged?.Invoke(0f);
    }
}