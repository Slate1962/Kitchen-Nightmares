using System;
using System.Collections.Generic;
using UnityEngine;

public class Plate : CarryableCC
{
    [Serializable]
    public struct FoodVisual
    {
        public FoodSO food;        // which food this child represents
        public GameObject visual;  // the child object on the plate
        public float layerHeight;  // how tall this piece is
    }

    // The order of this list is the stack order, bottom to top
    [SerializeField] private List<FoodVisual> visuals;
    [SerializeField] private float baseHeight = 0f; // plate surface height, relative to the plate

    private List<FoodSO> foods = new List<FoodSO>();

    public IReadOnlyList<FoodSO> Foods => foods;

    void Start()
    {
        RefreshStack();
    }

    public bool CanAddFood(FoodObjects food)
    {
        FoodSO so = food.GetKitchenObjectSO();

        if (!so.canBePlated) return false;
        if (foods.Contains(so)) return false;   // no duplicates
        return FindIndex(so) >= 0;              // plate must know how to show it
    }

    public bool TryAddFood(FoodObjects food)
    {
        if (!CanAddFood(food)) return false;

        foods.Add(food.GetKitchenObjectSO());
        RefreshStack();

        food.DestroySelf(); // the loose object is no longer needed
        return true;
    }

    public void ClearContents()
    {
        foods.Clear();
        RefreshStack();
    }

    // Shows only the foods on the plate, in list order, stacked without gaps
    private void RefreshStack()
    {
        float height = baseHeight;

        foreach (FoodVisual v in visuals)
        {
            bool onPlate = foods.Contains(v.food);
            v.visual.SetActive(onPlate);
            if (!onPlate) continue;

            Vector3 p = v.visual.transform.localPosition;
            p.y = height + v.layerHeight; // pivot in the middle of the mesh
            v.visual.transform.localPosition = p;

            height += v.layerHeight;
        }
    }

    private int FindIndex(FoodSO so)
    {
        for (int i = 0; i < visuals.Count; i++)
        {
            if (visuals[i].food == so) return i;
        }
        return -1;
    }
}