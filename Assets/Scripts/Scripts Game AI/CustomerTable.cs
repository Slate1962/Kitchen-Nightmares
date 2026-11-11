using UnityEngine;

public class CustomerTableCC : TableCC
{
    public bool isCustomerWaiting = true;

    public override void Interact(PlayerMovement player)
    {
        if (player.HeldItem != null)
        {
            Plate plate = player.HeldItem.GetComponent<Plate>();

            // Check if holding a plate with food, and a customer is waiting
            if (plate != null && plate.Foods.Count > 0 && isCustomerWaiting)
            {
                ServeCustomer();
                plate.ClearContents();

                return;
            }
        }
        base.Interact(player);
    }

    private void ServeCustomer()
    {
        isCustomerWaiting = false;
        Debug.Log("Food served to customer!");
    }

    //Call this method when a new customer group sits down
    public void ResetCustomerWaiting()
    {
        isCustomerWaiting = true;
    }
}