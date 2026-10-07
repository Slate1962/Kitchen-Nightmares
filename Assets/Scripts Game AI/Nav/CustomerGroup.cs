using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;


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
    public GameObject CustomerPrefab;

    private TableManager tableManager;
    private Table assignedTable;
    private float waitCheckTimer = 0.5f;
    private float checkInterval = 2f;

    private List<NavMeshAgent> agents = new List<NavMeshAgent>();

    private void Start()
    {
        tableManager = FindAnyObjectByType<TableManager>();
        groupSize = Random.Range(1, 5);

        SpawnCustomers();
    }

    private void SpawnCustomers()
    {
        for (int i = 0; i < groupSize; i++)
        {
            Vector3 randomSpawnOffset = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));

            Vector3 spawnPos = transform.position + randomSpawnOffset;

            GameObject newCustomer = Instantiate(CustomerPrefab, transform.position, Quaternion.identity, transform);

            NavMeshAgent agent = newCustomer.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agents.Add(agent);
            }
        }
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
                //Debug.Log($"Group of {groupSize} spawned. Agents in list: {agents.Count}");

                //NavMeshAgent[] agents = GetComponentsInChildren<NavMeshAgent>();

                for (int i = 0; i < agents.Count; i++)
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
