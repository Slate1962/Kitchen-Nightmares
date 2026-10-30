using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int coins = 0;
    public int reputation;
    private int maxReputation = 5;

    private void Start()
    {
        reputation = maxReputation;
    }

    private void Update()
    {
        if (reputation > maxReputation)
        {
            reputation--;
        }
    }
    public void AddCoins(int amount)
    {
        coins += amount;
        Debug.Log("Gained Coins, Current coins:" + coins);
    }
    public void GainReputation()
    {
        reputation++;
    }

    public void LoseReputation(int amount)
    {
        reputation -= amount;
        Debug.Log("Lost rep, Current Rep: " + reputation);

        if(reputation <= 0)
        {
            Debug.Log("Game Over");
        }    
    }
}
