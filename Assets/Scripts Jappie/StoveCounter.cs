using System;
using UnityEngine;

public class StoveCounter : MonoBehaviour, IHasProgress
{
    [SerializeField] private Transform cookingPoint;

    public event Action<float> OnProgressChanged;
    public event Action<bool> OnFoodPresenceChanged;

    private FoodObjects currentFood;
    private float cookingTimer;
    private float cookingTimerMax = 5f; // later uit een FoodSO halen

    void Update()
    {
        if (currentFood == null) return;

        cookingTimer += Time.deltaTime;
        OnProgressChanged?.Invoke(cookingTimer / cookingTimerMax);

        if (cookingTimer >= cookingTimerMax)
        {
            // hier het voedsel vervangen door de gebakken versie
            Debug.Log("Klaar met bakken!");
            OnFoodPresenceChanged?.Invoke(false);
            currentFood = null;
        }
    }

    public bool PlaceFood(FoodObjects food)
    {
        if (currentFood != null) return false;

        currentFood = food;
        food.transform.SetParent(cookingPoint);
        food.transform.localPosition = Vector3.zero;
        cookingTimer = 0f;
        OnProgressChanged?.Invoke(0f);
        OnFoodPresenceChanged?.Invoke(true);
        return true;
    }
}
