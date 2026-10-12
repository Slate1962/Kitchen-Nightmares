using UnityEngine;

public class ContainerCounter : InteractableCC
{
    [SerializeField] private FoodSO kitchenObjectSO; // sleep hier je FoodSO asset in

    public override bool CanInteract(PlayerMovement player)
    {
        return player.HeldItem == null;
    }

    public override void Interact(PlayerMovement player)
    {
        Transform spawned = Instantiate(kitchenObjectSO.prefab);
        FoodObjects food = spawned.GetComponent<FoodObjects>();

        player.PickUp(food);
        Debug.Log(food.GetKitchenObjectSO().foodName);
    }
}