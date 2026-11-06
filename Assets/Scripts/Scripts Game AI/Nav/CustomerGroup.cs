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
        Service,
        Eating,
        Leaving
    }

    public GroupState currentState = GroupState.waitingForTable;
    public int groupSize;

    [Header("Prefab")]
    public GameObject CustomerPrefab;

    [Header("Destination")]
    public Transform SpawnLocation;
    public Transform WaitingLocation;

    private TableManager tableManager;
    private Table assignedTable;
    private ScoreMenuManager scoreMenuManager;

    [Header("Timers")] //timers for different states of customer
    [SerializeField] private float orderDuration = 5f;
    [SerializeField] private float patience = 45f;
    [SerializeField] private float eatingDuration = 5f;

    [Header("Orders")] // NIEUW
    [SerializeField] private List<OrderSO> possibleOrders; // sleep hier je OrderSO assets in

    [Header("CustomerStatus")]
    public bool isSatisfied = false;

    //timers for prevention of constant frame running code
    private float waitCheckTimer = 0.5f;
    private float checkInterval = 2f;
    private float stateTimer = 0f;

    private List<NavMeshAgent> agents = new List<NavMeshAgent>();

    // NIEUW
    private CustomerTableCC customerTable;
    private OrderSO currentOrder;

    private void Start()
    {
        tableManager = FindAnyObjectByType<TableManager>();
        scoreMenuManager = FindAnyObjectByType<ScoreMenuManager>();
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
                    if (isSatisfied)
                    {
                        // GEWIJZIGD: beloning uit de bestelling, 5 als die ontbreekt
                        scoreMenuManager.AddCoins(currentOrder != null ? currentOrder.reward : 5);
                        scoreMenuManager.GainReputation();
                    }
                    else
                    {
                        scoreMenuManager.LoseReputation(1);
                    }
                    Destroy(gameObject);
                }
                break;
        }
    }

    private void HandleWaiting()
    {
        foreach (NavMeshAgent agent in agents)
        {
            if (agent.isOnNavMesh && SpawnLocation != null)
            {
                agent.SetDestination(WaitingLocation.position);
            }
        }

        waitCheckTimer -= Time.deltaTime;
        if (waitCheckTimer <= 0)
        {
            assignedTable = tableManager.RequestTable(groupSize, transform.position);

            if (assignedTable != null) //check if tables are available
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

    // GEWIJZIGD: kiest een bestelling en zet die op de tafel
    private void HandleOrdering()
    {
        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0)
        {
            customerTable = assignedTable.GetComponent<CustomerTableCC>();
            if (customerTable == null)
            {
                Debug.LogError(assignedTable.name + " heeft geen CustomerTableCC!");
                return;
            }

            if (possibleOrders == null || possibleOrders.Count == 0)
            {
                Debug.LogError("Geen OrderSO's ingesteld op " + name);
                return;
            }

            currentOrder = possibleOrders[Random.Range(0, possibleOrders.Count)];
            customerTable.SetOrder(currentOrder);
            customerTable.OnOrderServed += OnServed;

            Debug.Log("Group ordered " + currentOrder.orderName + ", waiting for food");
            currentState = GroupState.Service;
            stateTimer = patience;
        }
    }

    // GEWIJZIGD: wacht alleen nog op de patience, het serveren gaat via OnServed
    private void HandleService()
    {
        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0)
        {
            Debug.Log("Waited too long for food");

            ReleaseTable();
            if (assignedTable != null) assignedTable.ClearTable();

            SendAgentsAway();
            currentState = GroupState.Leaving;
        }
    }

    // GEWIJZIGD: test via het context-menu zonder echt bord
    [ContextMenu("Deliver Food")]
    public void ReceiveFood()
    {
        OnServed(currentOrder);
    }

    // NIEUW: wordt aangeroepen door CustomerTableCC als het juiste bord is geserveerd
    private void OnServed(OrderSO order)
    {
        if (customerTable != null) customerTable.OnOrderServed -= OnServed;

        if (currentState == GroupState.Service)
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
            ReleaseTable();
            if (assignedTable != null) assignedTable.ClearTable();

            isSatisfied = true;

            SendAgentsAway();
            currentState = GroupState.Leaving;
        }
    }

    // NIEUW: tafel loskoppelen van deze groep
    private void ReleaseTable()
    {
        if (customerTable != null)
        {
            customerTable.OnOrderServed -= OnServed;
            customerTable.SetOrder(null);
        }
    }

    // NIEUW: dezelfde loop die eerder op twee plekken stond
    private void SendAgentsAway()
    {
        foreach (NavMeshAgent agent in agents)
        {
            if (agent.isOnNavMesh && SpawnLocation != null)
            {
                agent.SetDestination(SpawnLocation.position);
            }
        }
    }

    // NIEUW: als de groep verdwijnt terwijl hij nog luistert
    private void OnDestroy()
    {
        if (customerTable != null) customerTable.OnOrderServed -= OnServed;
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