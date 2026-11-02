using UnityEngine;

public class TableCC : InteractableCC
{
    public CarryableCC heldItem;
    public Transform placePoint;

    public override bool CanInteract(PlayerMovement player)
    {
        CarryableCC held = player.HeldItem;

        if (held != null)
        {
            if (heldItem == null) return true;                 // place it down
            return CanCombine(held, heldItem);                 // plate + food
        }

        return heldItem != null;                               // take it back
    }

    public override void Interact(PlayerMovement player)
    {
        CarryableCC held = player.HeldItem;

        if (held != null && heldItem != null)
        {
            TryGetPlateAndFood(held, heldItem, out Plate plate, out FoodObjects food);

            if (held is Plate)
            {
                heldItem = null;          // food leaves the table, goes onto the plate in your hands
            }
            else
            {
                player.Release();         // food leaves your hands, goes onto the plate on the table
            }

            plate.TryAddFood(food);
        }
        else if (held != null)
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

    private bool CanCombine(CarryableCC a, CarryableCC b)
    {
        return TryGetPlateAndFood(a, b, out Plate plate, out FoodObjects food)
            && plate.CanAddFood(food);
    }

    private bool TryGetPlateAndFood(CarryableCC a, CarryableCC b, out Plate plate, out FoodObjects food)
    {
        plate = (a as Plate) ?? (b as Plate);
        food = (a as FoodObjects) ?? (b as FoodObjects);
        return plate != null && food != null;
    }
}