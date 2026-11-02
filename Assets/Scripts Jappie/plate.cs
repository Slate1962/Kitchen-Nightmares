using System;
using System.Collections.Generic;
using UnityEngine;

public class Plate : CarryableCC
{
    [Serializable]
    public struct FoodVisual
    {
        public FoodSO food;        // which food this child represents
        public GameObject visual;  // the disabled child on the plate
    }

    [SerializeField] private List<FoodVisual> visuals;

    private List<FoodSO> foods = new List<FoodSO>();

    public IReadOnlyList<FoodSO> Foods => foods;

    void Start()
    {
        foreach (FoodVisual v in visuals) v.visual.SetActive(false);
    }

    public bool CanAddFood(FoodObjects food)
    {
        FoodSO so = food.GetKitchenObjectSO();

        if (!so.canBePlated) return false;
        if (foods.Contains(so)) return false;   // no duplicates
        return FindVisual(so) != null;          // plate must know how to show it
    }

    public bool TryAddFood(FoodObjects food)
    {
        if (!CanAddFood(food)) return false;

        FoodSO so = food.GetKitchenObjectSO();
        FindVisual(so).SetActive(true);
        foods.Add(so);

        food.DestroySelf(); // the loose object is no longer needed
        return true;
    }

    private GameObject FindVisual(FoodSO so)
    {
        foreach (FoodVisual v in visuals)
        {
            if (v.food == so) return v.visual;
        }
        return null;
    }
}