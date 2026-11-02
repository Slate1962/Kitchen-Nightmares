using UnityEngine;
using UnityEngine.InputSystem;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] private int startingMoney = 100;
    [SerializeField] private UImoney uiMoney;
    [SerializeField] private int currentMoney;

    void Start()
    {
        currentMoney = startingMoney;
        uiMoney.MoneyText(currentMoney);
    }

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Getmoney();
        }
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        uiMoney.MoneyText(currentMoney);
    }

public bool SpendMoney(int amount)
{
    if (currentMoney >= amount)
    {
        currentMoney -= amount;
        uiMoney.MoneyText(currentMoney);
        return true;
    }

    Debug.Log("Not enough money!");
    return false;
}

    public void Getmoney()
    {
        AddMoney(50);
        Debug.Log("Debug: +50 money");
    }
}