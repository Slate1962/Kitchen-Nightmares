using UnityEngine;
using UnityEngine.AI;


public class CustomerGroup : MonoBehaviour
{
    public enum GroupState
    {
        waitingForTable,
        walkingToTable,
        Ordering,
        Eating,
        Leaving
    }

    public GroupState currentState = GroupState.waitingForTable;
    public int groupSize;

    private TableManager tableManager;
    private Table assignedTable;
    private float waitCheckTimer = 0f;
    private float checkInterval = 2f;

    private void Start()
    {
        tableManager = FindAnyObjectByType<TableManager>();
        groupSize = Random.Range(1, 5);
    }

    private void Update()
    {
        switch (currentState)
        {
            case GroupState.waitingForTable:
                HandleWaiting();
                break;
        }
    }

    private void HandleWaiting()
    {
        waitCheckTimer -= Time.deltaTime;
        if (waitCheckTimer <= 0)
        {
            assignedTable = tableManager.RequestTable(groupSize);

            if(assignedTable != null)
            {
                currentState = GroupState.walkingToTable;

                NavMeshAgent[] agents = GetComponentsInChildren<NavMeshAgent>();

                for (int i = 0; i < agents.Length; i++)
                {
                    if (i < assignedTable.Seats.Length)
                    {
                        agents[i].SetDestination(assignedTable.Seats[i].position);
                    }
                }
            }
            else
            {
                waitCheckTimer = checkInterval;
            }
        }
    }
}
