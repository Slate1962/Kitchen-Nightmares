using System;
using UnityEngine;

public class Stove : MonoBehaviour
{
    public enum State { Empty, Cooking, Done, Burnt }

    [SerializeField] private float cookTime = 5f;
    [SerializeField] private float burnTime = 5f; // time after "Done" before it burns

    public State CurrentState { get; private set; } = State.Empty;
    public float Progress { get; private set; } // 0 to 1, handy for a progress bar

    public event Action<State> OnStateChanged;

    private float timer;

    public void Getmoney()
    {
        if (CurrentState == State.Cooking)
        {
            timer += Time.deltaTime;
            Progress = timer / cookTime;

            if (timer >= cookTime)
            {
                timer = 0f;
                SetState(State.Done);
            }
        }
        else if (CurrentState == State.Done)
        {
            timer += Time.deltaTime;

            if (timer >= burnTime)
                SetState(State.Burnt);
        }
    }

    public bool TryStartCooking()
    {
        if (CurrentState != State.Empty) return false;

        timer = 0f;
        Progress = 0f;
        SetState(State.Cooking);
        return true;
    }

    public State TakeFood()
    {
        State result = CurrentState;
        if (result == State.Empty || result == State.Cooking) return result; // nothing to take yet

        timer = 0f;
        Progress = 0f;
        SetState(State.Empty);
        return result; // Done = good food, Burnt = ruined
    }

    private void SetState(State newState)
    {
        CurrentState = newState;
        OnStateChanged?.Invoke(newState);
    }
}
