using System;
using UnityEngine;

public class StoveCounter : InteractableCC, IHasProgress
{
    [SerializeField] private Transform cookingPoint;
    [SerializeField] private float cookingTimerMax = 5f; // later uit FoodSO halen

    public event Action<float> OnProgressChanged;    // 0 tot 1
    public event Action<bool> OnFoodPresenceChanged; // true = balk tonen

    private FoodObjects currentFood;
    private float cookingTimer;
    private bool isCooked;

    // E: neerleggen of terugpakken
    public override bool CanInteract(PlayerMovement player)
    {
        if (player.HeldItem != null)
        {
            FoodObjects food = player.HeldItem as FoodObjects;
            return currentFood == null && food != null && CanBeCooked(food);
        }

        return currentFood != null;
    }

    public override void Interact(PlayerMovement player)
    {
        if (player.HeldItem != null)
            PlaceFood((FoodObjects)player.Release());
        else
            player.PickUp(TakeFood());
    }

    void Update()
    {
        if (currentFood == null || isCooked) return;

        cookingTimer += Time.deltaTime;
        OnProgressChanged?.Invoke(cookingTimer / cookingTimerMax);

        if (cookingTimer >= cookingTimerMax)
        {
            FoodSO friedSO = currentFood.GetKitchenObjectSO().friedVersion;
            currentFood.DestroySelf();

            Transform spawned = Instantiate(friedSO.prefab, cookingPoint);
            spawned.localPosition = Vector3.zero;
            currentFood = spawned.GetComponent<FoodObjects>();

            isCooked = true;
            ResetProgress();
            OnFoodPresenceChanged?.Invoke(false); // klaar: balk weg
        }
    }

    private void PlaceFood(FoodObjects food)
    {
        currentFood = food;
        isCooked = false;
        food.transform.SetParent(cookingPoint);
        food.transform.localPosition = Vector3.zero;
        food.transform.localRotation = Quaternion.identity;

        ResetProgress();
        OnFoodPresenceChanged?.Invoke(true);
    }

    private FoodObjects TakeFood()
    {
        FoodObjects food = currentFood;
        currentFood = null;
        isCooked = false;
        ResetProgress();
        OnFoodPresenceChanged?.Invoke(false);
        return food;
    }

    private bool CanBeCooked(FoodObjects food)
    {
        return food.GetKitchenObjectSO().friedVersion != null;
    }

    private void ResetProgress()
    {
        cookingTimer = 0f;
        OnProgressChanged?.Invoke(0f);
    }
}