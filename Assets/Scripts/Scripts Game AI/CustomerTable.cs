using UnityEngine;

public class CustomerTableCC : TableCC
{
    public bool isCustomerWaiting = true;

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

        // If the code above hits "return;", it completely skips this bottom line.
        // If it doesn't hit "return;", it does normal table placement.
        base.Interact(player);
    }

    private void ServeCustomer()
    {
        isCustomerWaiting = false;
        Debug.Log("Food served to customer!");
    }
}