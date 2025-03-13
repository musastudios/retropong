using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOver : MonoBehaviour
{
    [Header("UI References")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI winnerText;
    public TextMeshProUGUI player1ScoreText;
    public TextMeshProUGUI player2ScoreText;

    private void Start()
    {
        // Hide game over panel initially
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        // Subscribe to game state change events
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from events
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
        }
    }

    private void HandleGameStateChanged(GameManager.GameState newState)
    {
        if (newState == GameManager.GameState.GameOver)
        {
            ShowGameOverScreen();
        }
    }

    private void ShowGameOverScreen()
    {
        // Show game over panel
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        // Update UI with final scores
        if (GameManager.Instance != null)
        {
            int player1Score = GameManager.Instance.Player1Score;
            int player2Score = GameManager.Instance.Player2Score;

            // Update score texts
            if (player1ScoreText != null)
            {
                player1ScoreText.text = player1Score.ToString();
            }

            if (player2ScoreText != null)
            {
                player2ScoreText.text = player2Score.ToString();
            }

            // Update winner text
            if (winnerText != null)
            {
                if (player1Score > player2Score)
                {
                    winnerText.text = "Player 1 Wins!";
                }
                else if (player2Score > player1Score)
                {
                    winnerText.text = "Player 2 Wins!";
                }
                else
                {
                    winnerText.text = "It's a Tie!";
                }
            }
        }
    }

    public void PlayAgain()
    {
        // Reload current scene
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void ReturnToMainMenu()
    {
        // Load main menu
        SceneManager.LoadScene("MainMenu");
    }
} 