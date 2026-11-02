using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ScoreMenuManager : MonoBehaviour
{
    MenuManager menuManager;

    public int coins = 0;
    public int reputation;
    [SerializeField]private int maxReputation = 3;

    private void Start()
    {
        reputation = maxReputation;
        Time.timeScale = 1f;
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
            menuManager.GameOver();
        }    
    }

}
