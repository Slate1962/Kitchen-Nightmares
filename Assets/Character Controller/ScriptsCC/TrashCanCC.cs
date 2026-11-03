using UnityEngine;

public class TrashCanCC : InteractableCC
{
    public override bool CanInteract(PlayerMovement player)
    {
        if (player.HeldItem == null)
        {
            return false;
        }

        Plate plate = player.HeldItem.GetComponent<Plate>();

        if (plate != null)
        {
            return plate.Foods.Count > 0;
        }

        return true;
    }

    public override void Interact(PlayerMovement player)
    {
        Plate plate = player.HeldItem.GetComponent<Plate>();

        if (plate != null)
        {
            plate.ClearContents();
        }
        else
        {
            CarryableCC item = player.Release();
            Destroy(item.gameObject);
        }
    }
}