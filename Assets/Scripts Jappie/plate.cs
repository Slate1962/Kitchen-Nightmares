using System.Collections.Generic;
using UnityEngine;

public class Plate : CarryableCC
{
    [SerializeField] private Transform plateIngredientPoint; // empty child on the plate
    [SerializeField] private float stackHeight = 0.05f;      // vertical gap between layers

    private List<FoodObjects> foods = new List<FoodObjects>();

    public IReadOnlyList<FoodObjects> Foods => foods;

    // No duplicates: one tomato per plate
    public bool CanAddFood(FoodObjects food)
    {
        FoodSO so = food.GetKitchenObjectSO();
        foreach (FoodObjects f in foods)
        {
            if (f.GetKitchenObjectSO() == so) return false;
        }
        return true;
    }

    public bool TryAddFood(FoodObjects food)
    {
        if (!CanAddFood(food)) return false;

        foods.Add(food);
        food.transform.SetParent(plateIngredientPoint);
        food.transform.localPosition = Vector3.up * stackHeight * (foods.Count - 1);
        food.transform.localRotation = Quaternion.identity;
        return true;
    }
}
