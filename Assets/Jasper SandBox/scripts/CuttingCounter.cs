using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CuttingCounter : MonoBehaviour, IHasProgress
{
    [SerializeField] private Transform cuttingPoint;
    [SerializeField] private FoodSO debugFoodSO; // alleen om te testen met Space

    public event Action<float> OnProgressChanged;    // 0 tot 1
    public event Action<bool> OnFoodPresenceChanged; // true = balk tonen

    private FoodObjects currentFood;
    private int cuttingProgress;
    private bool isSliced;

    void Update()
    {
        if (Keyboard.current.backspaceKey.wasPressedThisFrame) SliceFood();

        // Simuleert dat de speler iets neerlegt
        if (Keyboard.current.spaceKey.wasPressedThisFrame && debugFoodSO != null)
        {
            Transform spawned = Instantiate(debugFoodSO.prefab);
            if (!PlaceFood(spawned.GetComponent<FoodObjects>()))
                Destroy(spawned.gameObject); // er lag al iets, dus geen losse kopie laten rondzweven
        }
    }

    public bool HasFood() => currentFood != null;

    public bool PlaceFood(FoodObjects food)
    {
        if (currentFood != null) return false;

        currentFood = food;
        isSliced = false;
        food.transform.SetParent(cuttingPoint);
        food.transform.localPosition = Vector3.zero;
        food.transform.localRotation = Quaternion.identity;

        ResetProgress();
        OnFoodPresenceChanged?.Invoke(CanBeSliced(food)); // balk tonen
        return true;
    }

    public FoodObjects TakeFood()
    {
        if (currentFood == null) return null;

        FoodObjects food = currentFood;
        currentFood = null;
        isSliced = false;
        ResetProgress();
        OnFoodPresenceChanged?.Invoke(false); // balk verbergen
        return food;
    }

    private bool CanBeSliced(FoodObjects food)
    {
        return food.GetKitchenObjectSO().slicedVersion != null;
    }

    public void SliceFood()
    {
        if (currentFood == null || isSliced) return;

        FoodSO currentSO = currentFood.GetKitchenObjectSO();
        if (currentSO.slicedVersion == null) return;

        cuttingProgress++;
        OnProgressChanged?.Invoke((float)cuttingProgress / currentSO.cuttingProgressMax);
        Debug.Log("Snee " + cuttingProgress + "/" + currentSO.cuttingProgressMax);

        if (cuttingProgress >= currentSO.cuttingProgressMax)
        {
            FoodSO slicedSO = currentSO.slicedVersion;
            currentFood.DestroySelf();

            Transform spawned = Instantiate(slicedSO.prefab, cuttingPoint);
            spawned.localPosition = Vector3.zero;
            currentFood = spawned.GetComponent<FoodObjects>();

            isSliced = true;
            ResetProgress();
            OnFoodPresenceChanged?.Invoke(false); // klaar: balk weg
        }
    }

    private void ResetProgress()
    {
        cuttingProgress = 0;
        OnProgressChanged?.Invoke(0f);
    }
}