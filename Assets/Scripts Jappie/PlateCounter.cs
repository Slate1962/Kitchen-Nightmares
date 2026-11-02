using UnityEngine;

public class PlateCounter : InteractableCC
{
    [SerializeField] private Plate platePrefab;

    public override bool CanInteract(PlayerMovement player)
    {
        return player.HeldItem == null;
    }

    public override void Interact(PlayerMovement player)
    {
        Plate plate = Instantiate(platePrefab);
        player.PickUp(plate);
    }
}