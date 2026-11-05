using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DayManager : MonoBehaviour
{
    [Header("Difficulty")]
    [SerializeField] private int baseCustomers = 3;          // klanten op dag 1
    [SerializeField] private int extraCustomersPerDay = 2;   // erbij per dag

    public int CurrentDay { get; private set; } = 1;
    public int CustomersToday => baseCustomers + extraCustomersPerDay * (CurrentDay - 1);
    public int CustomersLeft => CustomersToday - customersDone;

    public event Action<int> OnDayStarted;   // dagnummer
    public event Action<int> OnDayEnded;     // dagnummer

    private int customersDone;
    private bool dayRunning;

    void Start()
    {
        StartDay();
    }

    void Update()
    {
        // tijdelijke debugtoets: N = volgende dag
        if (!dayRunning && Keyboard.current.nKey.wasPressedThisFrame)
        {
            NextDay();
        }
    }

    // Laat de klanten-AI dit aanroepen zodra een klant weggaat (tevreden of niet)
    public void CustomerFinished()
    {
        if (!dayRunning) return;

        customersDone++;
        if (customersDone >= CustomersToday) EndDay();
    }

    public void NextDay()
    {
        if (dayRunning) return;
        CurrentDay++;
        StartDay();
    }

    private void StartDay()
    {
        customersDone = 0;
        dayRunning = true;
        Debug.Log("Dag " + CurrentDay + ": " + CustomersToday + " klanten");
        OnDayStarted?.Invoke(CurrentDay);
    }

    private void EndDay()
    {
        dayRunning = false;
        Debug.Log("Dag " + CurrentDay + " voorbij. Druk N voor de volgende dag.");
        OnDayEnded?.Invoke(CurrentDay);
    }
}
