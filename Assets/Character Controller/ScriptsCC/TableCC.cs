using UnityEngine;

public class TableCC : InteractableCC
{
    public CarryableCC heldItem;
    public Transform placePoint;

    public override bool CanInteract(PlayerMovement player)
    {
        if (player.HeldItem != null)
        {
            return heldItem == null; 
        }

        return heldItem != null;
    }

    public override void Interact(PlayerMovement player) 
    {
        if (player.HeldItem != null)
        {
            heldItem = player.Release(); 
            heldItem.transform.SetParent(placePoint);
            heldItem.transform.localPosition = Vector3.zero;
            heldItem.transform.localRotation = Quaternion.identity; 
        }

        else 
        {
            CarryableCC item = heldItem;
            heldItem = null;
            player.PickUp(item);
        }
    }
}