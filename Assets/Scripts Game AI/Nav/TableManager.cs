using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TableManager : MonoBehaviour
{
    public List<Table> allTables;

    public Table RequestTable(int groupSize, Vector3 requesterPosition)
    {
        Table bestTable = null;
        int smallestCount = int.MaxValue;
        float closestDistance = float.MaxValue;

        foreach (Table table in allTables)
        {
            if(!table.isOccupied && table.Seats.Length >= groupSize)
            {
                float distanceToTable = Vector3.Distance(requesterPosition, table.transform.position);

                if(table.Seats.Length < smallestCount)
                {   
                    bestTable = table;
                    smallestCount = table.Seats.Length;
                    closestDistance = distanceToTable;
                }
                else if(table.Seats.Length == smallestCount && distanceToTable < closestDistance)
                {
                    bestTable = table;
                    closestDistance = distanceToTable;
                }
            }
        }

        if (bestTable != null)
        {
            bestTable.ReserveSeat(groupSize);
            return bestTable;
        }
        return null;
    }
}
