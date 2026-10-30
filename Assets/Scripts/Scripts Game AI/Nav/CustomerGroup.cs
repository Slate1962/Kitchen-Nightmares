using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Data;


public class CustomerGroup : MonoBehaviour
{
    public enum GroupState //defining states for customer
    {
        waitingForTable,
        walkingToTable,
        Ordering,
        Service,
        Delivered,
        Eating,
        Leaving
    }

    public GroupState currentState = GroupState.waitingForTable;
    public int groupSize;

    [Header("Prefab")]
    public GameObject CustomerPrefab;

    [Header("Destination")]
    public Transform SpawnLocation;

    private TableManager tableManager;
    private Table assignedTable;

    [Header("Timers")] //timers for different states of customer
    [SerializeField] private float orderDuration = 5f;
    [SerializeField] private float patience = 45f;
    [SerializeField] private float eatingDuration = 5f;

    private bool foundTable = false;

    //timers for prevention of constant frame running code
    private float waitCheckTimer = 0.5f;
    private float checkInterval = 2f;
    private float stateTimer = 0f;

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
            case GroupState.walkingToTable:
                if (CheckAllCustomersArrived())
                {
                    currentState = GroupState.Ordering;
                    stateTimer = orderDuration;
                }
                break;
            case GroupState.Ordering:
                HandleOrdering();
                break;
            case GroupState.Service:
                HandleService();
                break;
            case GroupState.Eating:
                HandleEating();
                break;
            case GroupState.Leaving:
                if (CheckAllCustomersArrived())
                {
                    Destroy(gameObject);
                }
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
                            foundTable = true;
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
    private void HandleOrdering()
    {
        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0)
        {
            Debug.Log("Group ordered, waiting for food");
            currentState = GroupState.Service;
            stateTimer = patience;
        }
    }

    private void HandleService()
    {
        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0)
        {
            Debug.Log("Waited too long for food");
            if (assignedTable != null) assignedTable.ClearTable();
            {
                foreach (NavMeshAgent agent in agents)
                {
                    if (agent.isOnNavMesh && SpawnLocation != null)
                    {
                        agent.SetDestination(SpawnLocation.position);
                    }
                }
                currentState = GroupState.Leaving;
            }
        }
    }

    public void ReceiveFood()
    {
        if(currentState == GroupState.Service)
        {
            Debug.Log("Food Arrived");
            currentState = GroupState.Eating;
            stateTimer = eatingDuration;
        }
    }

    private void HandleEating()
    {
        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0)
        {
            if (assignedTable != null) assignedTable.ClearTable();

            foreach (NavMeshAgent agent in agents)
            {
                if (agent.isOnNavMesh && SpawnLocation != null)
                {
                    agent.SetDestination(SpawnLocation.position);
                }
            }
            currentState = GroupState.Leaving;
        }
    }
    private bool CheckAllCustomersArrived()
    {
        float arrivalbuffer = (currentState == GroupState.Leaving) ? 2.0f : 5.0f;

        foreach (NavMeshAgent agent in agents)
        {
            if (agent.pathPending) return false;

            float distancetoTarget = Vector3.Distance(agent.transform.position, agent.destination);
            if (distancetoTarget > agent.stoppingDistance + arrivalbuffer)
                return false;
        }
        return true;
    }
}
