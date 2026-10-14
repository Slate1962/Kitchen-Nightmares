using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;


public class CustomerGroup : MonoBehaviour
{
    public enum GroupState //defining states for customer
    {
        waitingForTable,
        walkingToTable,
        Ordering,
        Eating,
        Leaving
    }

    public GroupState currentState = GroupState.waitingForTable;
    public int groupSize;

    [Header("Prefab")]
    public GameObject CustomerPrefab;

    [Header("ExitDestination")]
    public Transform exitLocation;

    private TableManager tableManager;
    private Table assignedTable;

    [Header("Timers")] //timers for different states of customer
    [SerializeField] private float orderDuration = 2f;
    [SerializeField] private float patience = 10f;
    [SerializeField] private float eatingDuration = 5f;

    //timers for prevention of constant frame running code
    private float waitCheckTimer = 0.5f;
    private float checkInterval = 2f;

    private List<NavMeshAgent> agents = new List<NavMeshAgent>();

    private void Start()
    {
        tableManager = FindAnyObjectByType<TableManager>();
        groupSize = Random.Range(1, 5);

        SpawnCustomers();
    }

    private void SpawnCustomers() //customer spawning system with a random offset to avoid clumping together
    {
        for (int i = 0; i < groupSize; i++)
        {
            Vector3 randomSpawnOffset = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            Vector3 spawnPos = transform.position + randomSpawnOffset;

            GameObject newCustomer = Instantiate(CustomerPrefab, spawnPos, Quaternion.identity, transform);

            NavMeshAgent agent = newCustomer.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agents.Add(agent);
            }
        }
    }

    private void Update()
    {
        // state machine
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
            assignedTable = tableManager.RequestTable(groupSize, transform.position);

            if(assignedTable != null) //check if tables are available
            {
                currentState = GroupState.walkingToTable; //stop looking for table

                for (int i = 0; i < agents.Count; i++) //check for all agents in group
                {
                    if (i < assignedTable.Seats.Length) //check for available seats
                    {
                        if (agents[i].isOnNavMesh) //check whether or not agent is on navmesh
                        {
                            agents[i].SetDestination(assignedTable.Seats[i].position); //tell customer to go to their seat
                        }
                        else
                        {
                            Debug.LogError("Customer failed to path since not on Navmesh");
                        }
                    }
                }
            }
            else
            {
                waitCheckTimer = checkInterval; //if no table available check again after timer
            }
        }
    }
}
