using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public int coins = 0;
    public int reputation;
    [SerializeField]private int maxReputation = 3;
    private bool isPaused = false;

    public GameObject GameEndScreen;
    public GameObject PauseScreen;

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
            GameOver();
        }    
    }
    public void OnPauseMenu(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }
    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;
        PauseScreen.SetActive(true);
    }
    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;
        PauseScreen.SetActive(false);
    }
    public void GameOver()
    {
         Time.timeScale = 0f;
         GameEndScreen.SetActive(true);
         Debug.Log("Game Over");
    }
    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }
}
