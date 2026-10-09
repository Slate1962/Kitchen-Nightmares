using UnityEngine;

public class Crate : InteractableCC
{
    public IngredientType ingredientType;
    public bool needsChopping;
    public bool needsCooking;
    public GameObject ingredientPrefab;

    public override bool CanInteract(PlayerMovement player)
    {
        return player.HeldItem == null;
    }

    public override void Interact(PlayerMovement player)
    {
        GameObject newObject = Instantiate(ingredientPrefab);
        Ingredient ingredient = newObject.GetComponent<Ingredient>();

        ingredient.type = ingredientType;
        ingredient.needsChopping = needsChopping;
        ingredient.needsCooking = needsCooking;

        player.PickUp(ingredient);
    }
}