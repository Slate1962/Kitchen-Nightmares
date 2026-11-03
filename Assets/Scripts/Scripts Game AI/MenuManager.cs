using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private bool isPaused = false;
    public GameObject PauseScreen;

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void OnPauseMenu(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (SceneManager.GetActiveScene().name != "Demo1") return;

        if (isPaused)
            Resume();
        else
            Pause();

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

    public void ReloadScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string scene = currentScene.name;
        SceneManager.LoadScene(scene);
    }

    public void GameQuit()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}
