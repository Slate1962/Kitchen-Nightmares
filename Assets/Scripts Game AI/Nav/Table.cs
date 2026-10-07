using UnityEngine;

public class Table : MonoBehaviour
{
    public Transform[] Seats;
    public bool isOccupied { get; private set; } = false;
    public int currentCustomerGroupSize { get; private set; } = 0;

    public bool ReserveSeat(int groupSize)
    {
        if(!isOccupied && groupSize <= Seats.Length)
        {
            isOccupied = true;
            currentCustomerGroupSize = groupSize;
            return true;
        }
        return false;
    }

    public void ClearTable()
    {
        isOccupied = false;
        currentCustomerGroupSize = 0;
    }
}
