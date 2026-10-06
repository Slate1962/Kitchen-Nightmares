using System.Collections.Generic;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    public List<Table> allTables;

    public Table RequestTable(int groupSize)
    {
        foreach (Table table in allTables)
        {
            if(table.ReserveSeat(groupSize))
            {
                return table;
            }
        }
        return null;
    }
}
