using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI player1ScoreText;
    public TextMeshProUGUI player2ScoreText;
    public TextMeshProUGUI winnerText;
    public GameObject gameOverPanel;

    private void Start()
    {
        // Subscribe to score change events
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnScoreChanged += UpdateScoreDisplay;
            GameManager.Instance.OnGameStateChanged += HandleGameStateChanged;
        }

        // Initialize score display
        UpdateScoreDisplay(0, 0);

        // Hide game over panel initially
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from events
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnScoreChanged -= UpdateScoreDisplay;
            GameManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
        }
    }

    private void UpdateScoreDisplay(int player1Score, int player2Score)
    {
        // Update score text
        if (player1ScoreText != null)
        {
            player1ScoreText.text = player1Score.ToString();
        }

        if (player2ScoreText != null)
        {
            player2ScoreText.text = player2Score.ToString();
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
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        if (winnerText != null)
        {
            // Determine winner
            int player1Score = GameManager.Instance.Player1Score;
            int player2Score = GameManager.Instance.Player2Score;

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