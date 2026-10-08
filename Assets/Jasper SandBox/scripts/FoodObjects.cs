using UnityEngine;

public class FoodObjects : MonoBehaviour
{
    [SerializeField] private FoodSO kitchenObjectSO;

    public FoodSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}