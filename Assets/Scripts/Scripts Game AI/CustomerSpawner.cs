using System.Collections;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DayManager dayManager;
    [SerializeField] private GameObject customerGroupPrefab; // Sleep hier je CustomerGroup prefab in
    [SerializeField] private Transform spawnLocation;
    [SerializeField] private Transform waitingLocation;

    [Header("Spawning Times")]
    [SerializeField] private float minSpawnDelay = 5f;
    [SerializeField] private float maxSpawnDelay = 20f;

    [Header("Settings")]
    [SerializeField] private bool spawnInfinitely = false;

    private Coroutine spawnCoroutine;

    private void Start()
    {
        // Zoek de DayManager als deze niet in de inspector is gesleept
        if (dayManager == null)
            dayManager = FindAnyObjectByType<DayManager>();

        // Luister naar de start van een nieuwe dag
        if (dayManager != null)
        {
            dayManager.OnDayStarted += HandleDayStarted;
            dayManager.OnDayEnded += HandleDayEnded;
        }
    }

    private void OnDestroy()
    {
        // Netjes event listeners opruimen
        if (dayManager != null)
        {
            dayManager.OnDayStarted -= HandleDayStarted;
            dayManager.OnDayEnded -= HandleDayEnded;
        }
    }

    private void HandleDayStarted(int day)
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }

        // Start het spawnen gebaseerd op het aantal klanten (groepen) van vandaag
        spawnCoroutine = StartCoroutine(SpawnRoutine(dayManager.CustomersToday));
    }

    private void HandleDayEnded(int day)
    {
        // Stop met spawnen als de dag vroegtijdig eindigt
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }
    }

    // Dit is nu je enige SpawnRoutine, die zowel de oneindige als de gelimiteerde logic afhandelt
    private IEnumerator SpawnRoutine(int amountToSpawn)
    {
        // If true, loop forever. If false, run your normal DayManager amount.
        while (spawnInfinitely || amountToSpawn > 0)
        {
            float waitTime = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(waitTime);

            SpawnGroup();

            if (!spawnInfinitely) amountToSpawn--;
        }
    }

    private void SpawnGroup()
    {
        // Spawn de groep op de spawn locatie
        GameObject groupObj = Instantiate(customerGroupPrefab, spawnLocation.position, Quaternion.identity);
        CustomerGroup newGroup = groupObj.GetComponent<CustomerGroup>();

        if (newGroup != null)
        {
            // Geef de locaties door aan de CustomerGroup zodat ze weten waar ze heen moeten
            newGroup.SpawnLocation = spawnLocation;
            newGroup.WaitingLocation = waitingLocation;
        }
    }
}