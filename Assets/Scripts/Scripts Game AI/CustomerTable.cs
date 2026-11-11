using UnityEngine;
using System; // Required for using Action

public class CustomerTableCC : TableCC
{
    public bool isCustomerWaiting = true;

    // 1. Add the missing order reference and event
    private OrderSO expectedOrder;
    public event Action<OrderSO> OnOrderServed;

    // 2. Add the SetOrder method called by CustomerGroup
    public void SetOrder(OrderSO order)
    {
        expectedOrder = order;
        isCustomerWaiting = (order != null);
    }

    public override void Interact(PlayerMovement player)
    {
        if (player.HeldItem != null)
        {
            Plate plate = player.HeldItem.GetComponent<Plate>();

            if (plate != null && plate.Foods.Count > 0 && isCustomerWaiting)
            {
                ServeCustomer();
                plate.ClearContents();

                // Added the return statement here based on your comment below
                return;
            }
        }

        // If the code above hits "return;", it completely skips this bottom line.
        // If it doesn't hit "return;", it does normal table placement.
        base.Interact(player);
    }

    private void ServeCustomer()
    {
        isCustomerWaiting = false;
        Debug.Log("Food served to customer!");

        // 3. Trigger the event to let CustomerGroup know the food arrived
        OnOrderServed?.Invoke(expectedOrder);
    }
}