using UnityEngine;

public class Crate : InteractableCC
{
    public FoodSO food;

    public override bool CanInteract(PlayerMovement player)
    {
        return player.HeldItem == null;
    }

    public override void Interact(PlayerMovement player)
    {
        if (food == null || food.prefab == null)
        {
            Debug.LogError(name + " has no food or no prefab assigned");
            return;
        }

        Transform foodTransform = Instantiate(food.prefab);
        Ingredient ingredient = foodTransform.GetComponent<Ingredient>();

        if (ingredient == null)
        {
            ingredient = foodTransform.gameObject.AddComponent<Ingredient>();
        }

        ingredient.food = food;
        player.PickUp(ingredient);
    }
}