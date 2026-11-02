using UnityEngine;

public class CuttingCounterUpgrade : MonoBehaviour
{
    [SerializeField] private CuttingCounter cuttingCounter;
    [SerializeField] private MoneyManager moneyManager;

    public void BuyUpgrade()
    {
        if (!cuttingCounter.CanUpgrade())
        {
            Debug.Log("Already at max level");
            return;
        }

        if (moneyManager.SpendMoney(cuttingCounter.GetUpgradeCost()))
        {
            cuttingCounter.Upgrade();
            Debug.Log("Upgrade purchased successfully");
        }
    }
}
