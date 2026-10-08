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

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        uiMoney.MoneyText(currentMoney);
    }

    public void SpendMoney(int amount)
    {
        if (currentMoney >= amount)
        {
            currentMoney -= amount;
            uiMoney.MoneyText(currentMoney);
        }
        else
        {
            Debug.Log("Not enough money!");
        }
    }

    

    public void Getmoney()
    {
    if (Keyboard.current.spaceKey.wasPressedThisFrame)
    Debug.Log("Debug: Space key pressed");
    {
        AddMoney(50);
        Debug.Log("Debug: +50 money");
    }
    }

    


}
