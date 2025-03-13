using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("UI References")]
    public GameObject pauseMenuPanel;

    private bool isPaused = false;

    private void Start()
    {
        // Hide pause menu initially
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }
    }

    private void Update()
    {
        // Check for pause input
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        
        if (isPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    public void PauseGame()
    {
        // Show pause menu
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(true);
        }
        
        // Pause game
        if (GameManager.Instance != null)
        {
            GameManager.Instance.PauseGame();
        }
        else
        {
            // Fallback if GameManager not available
            Time.timeScale = 0f;
        }
        
        isPaused = true;
    }

    public void ResumeGame()
    {
        // Hide pause menu
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }
        
        // Resume game
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResumeGame();
        }
        else
        {
            // Fallback if GameManager not available
            Time.timeScale = 1f;
        }
        
        isPaused = false;
    }

    public void RestartGame()
    {
        // Resume time scale
        Time.timeScale = 1f;
        
        // Reload current scene
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void ReturnToMainMenu()
    {
        // Resume time scale
        Time.timeScale = 1f;
        
        // Load main menu
        SceneManager.LoadScene("MainMenu");
    }
} 