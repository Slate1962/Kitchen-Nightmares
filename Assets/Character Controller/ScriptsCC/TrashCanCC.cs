using UnityEngine;

public class TrashCanCC : InteractableCC
{
    public override bool CanInteract(PlayerMovement player)
    {
        return player.HeldItem != null;
    }

    public override void Interact(PlayerMovement player)
    {
        CarryableCC item = player.Release();
        Destroy(item.gameObject);
    }
}