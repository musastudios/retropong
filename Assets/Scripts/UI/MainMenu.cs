using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Menu References")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;

    [Header("Game Settings")]
    public bool isSinglePlayer = true;

    private void Start()
    {
        // Show main menu, hide settings
        ShowMainMenu();
    }

    public void StartSinglePlayerGame()
    {
        isSinglePlayer = true;
        LoadGameScene();
    }

    public void StartTwoPlayerGame()
    {
        isSinglePlayer = false;
        LoadGameScene();
    }

    public void ShowSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private void LoadGameScene()
    {
        // Store game mode preference
        PlayerPrefs.SetInt("SinglePlayerMode", isSinglePlayer ? 1 : 0);
        PlayerPrefs.Save();
        
        // Load game scene
        SceneManager.LoadScene("GameScene");
    }
} 