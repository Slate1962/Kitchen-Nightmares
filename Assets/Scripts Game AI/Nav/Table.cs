using UnityEngine;

public class Table : MonoBehaviour
{
    public Transform[] Seats;
    public bool isOccupied { get; private set; } = false; //Check occupation of table, prevent other scripts from changing but be allowed to read
    public int currentCustomerGroupSize { get; private set; } = 0; //track amount of customers

    public bool ReserveSeat(int groupSize)
    {
        if(!isOccupied && groupSize <= Seats.Length) //check occupation and size
        {
            isOccupied = true;
            currentCustomerGroupSize = groupSize;
            return true;
        }
        return false;
    }

    public void ClearTable() //reset table for when customer leaves
    {
        isOccupied = false;
        currentCustomerGroupSize = 0;
    }
}
