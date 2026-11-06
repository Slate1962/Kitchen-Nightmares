using System;
using System.Linq;
using UnityEngine;

public class CustomerTableCC : TableCC
{
    public OrderSO currentOrder;   // ingesteld door CustomerGroup, null = geen klant
    public event Action<OrderSO> OnOrderServed;

    public bool isCustomerWaiting => currentOrder != null;

    // Aanroepen door de klanten-AI (met null om de bestelling te wissen)
    public void SetOrder(OrderSO order)
    {
        currentOrder = order;
    }

    public override bool CanInteract(PlayerMovement player)
    {
        Plate plate = player.HeldItem as Plate;

        // Een klant wacht en het bord klopt: serveren
        if (plate != null && isCustomerWaiting && Matches(plate, currentOrder))
            return true;

        // Anders gedraagt de tafel zich als een gewone tafel
        return base.CanInteract(player);
    }

    public override void Interact(PlayerMovement player)
    {
        Plate plate = player.HeldItem as Plate;

        if (plate != null && isCustomerWaiting && Matches(plate, currentOrder))
        {
            OrderSO served = currentOrder;
            currentOrder = null;
            plate.ClearContents();      // het lege bord blijft in de handen van de speler
            OnOrderServed?.Invoke(served);
            return;                     // geen base.Interact, anders ligt het bord op tafel
        }

        base.Interact(player);
    }

    private bool Matches(Plate plate, OrderSO order)
    {
        if (plate.Foods.Count != order.foods.Count) return false;
        return order.foods.All(f => plate.Foods.Contains(f));
    }
}