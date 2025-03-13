using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI player1ScoreText;
    public TextMeshProUGUI player2ScoreText;
    public TextMeshProUGUI gameStateText;

    private void Start()
    {
        // Subscribe to score change events
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnScoreChanged += UpdateScoreDisplay;
            GameManager.Instance.OnGameStateChanged += UpdateGameStateDisplay;
        }

        // Initialize score display
        UpdateScoreDisplay(0, 0);
        
        // Initialize game state display
        if (GameManager.Instance != null)
        {
            UpdateGameStateDisplay(GameManager.Instance.CurrentState);
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from events
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnScoreChanged -= UpdateScoreDisplay;
            GameManager.Instance.OnGameStateChanged -= UpdateGameStateDisplay;
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

    private void UpdateGameStateDisplay(GameManager.GameState newState)
    {
        if (gameStateText != null)
        {
            switch (newState)
            {
                case GameManager.GameState.MainMenu:
                    gameStateText.text = "";
                    break;
                case GameManager.GameState.Playing:
                    gameStateText.text = "";
                    break;
                case GameManager.GameState.Paused:
                    gameStateText.text = "PAUSED";
                    break;
                case GameManager.GameState.GameOver:
                    gameStateText.text = "GAME OVER";
                    break;
            }
        }
    }
} 