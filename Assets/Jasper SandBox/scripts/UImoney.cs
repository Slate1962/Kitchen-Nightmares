using UnityEngine;
using TMPro;

public class UImoney : MonoBehaviour
{
    
    [SerializeField] TMP_Text moneyText;


    public void MoneyText(int amount)
    {
        moneyText.text = amount.ToString();
    }

    void Start()
    {
        moneyText.text = "100";
    }

    
    void Update()
    {
        
    }
}
