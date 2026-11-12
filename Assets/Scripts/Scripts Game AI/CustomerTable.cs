using UnityEngine;
using System; // Required for using Action

public class CustomerTableCC : TableCC
{
    public bool isCustomerWaiting = true;

    // Add the missing order reference and event
    private OrderSO expectedOrder;
    public event Action<OrderSO> OnOrderServed;

    // Add the SetOrder method called by CustomerGroup
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

            }
        }
        base.Interact(player);
    }

    private void ServeCustomer()
    {
        isCustomerWaiting = false;
        Debug.Log("Food served to customer!");

        //Trigger the event to let CustomerGroup know the food arrived
        OnOrderServed?.Invoke(expectedOrder);
    }
}