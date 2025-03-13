using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton pattern
    public static GameManager Instance { get; private set; }

    // Game state
    public enum GameState { MainMenu, Playing, Paused, GameOver }
    public GameState CurrentState { get; private set; } = GameState.MainMenu;

    // Score tracking
    public int Player1Score { get; private set; } = 0;
    public int Player2Score { get; private set; } = 0;
    public int ScoreToWin = 10;

    // References to other game components
    public BallController Ball { get; private set; }
    public PlayerController Player1 { get; private set; }
    public PlayerController Player2 { get; private set; }
    public ScoreManager ScoreManager { get; private set; }

    // Events
    public delegate void GameStateChangedHandler(GameState newState);
    public event GameStateChangedHandler OnGameStateChanged;

    public delegate void ScoreChangedHandler(int player1Score, int player2Score);
    public event ScoreChangedHandler OnScoreChanged;

    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // Find references to game components
        Ball = FindObjectOfType<BallController>();
        Player1 = FindObjectsOfType<PlayerController>()[0];
        Player2 = FindObjectsOfType<PlayerController>()[1];
        ScoreManager = FindObjectOfType<ScoreManager>();
    }

    public void StartGame()
    {
        Player1Score = 0;
        Player2Score = 0;
        ChangeGameState(GameState.Playing);
        ResetBall();
        
        // Notify score change
        OnScoreChanged?.Invoke(Player1Score, Player2Score);
    }

    public void PauseGame()
    {
        if (CurrentState == GameState.Playing)
        {
            ChangeGameState(GameState.Paused);
            Time.timeScale = 0f;
        }
    }

    public void ResumeGame()
    {
        if (CurrentState == GameState.Paused)
        {
            ChangeGameState(GameState.Playing);
            Time.timeScale = 1f;
        }
    }

    public void EndGame()
    {
        ChangeGameState(GameState.GameOver);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        ChangeGameState(GameState.MainMenu);
    }

    public void PlayerScored(int playerNumber)
    {
        // Update score
        if (playerNumber == 1)
        {
            Player1Score++;
        }
        else if (playerNumber == 2)
        {
            Player2Score++;
        }

        // Notify score change
        OnScoreChanged?.Invoke(Player1Score, Player2Score);

        // Check for win condition
        if (Player1Score >= ScoreToWin || Player2Score >= ScoreToWin)
        {
            EndGame();
        }
        else
        {
            ResetBall();
        }
    }

    private void ResetBall()
    {
        if (Ball != null)
        {
            Ball.ResetBall();
        }
    }

    private void ChangeGameState(GameState newState)
    {
        CurrentState = newState;
        OnGameStateChanged?.Invoke(newState);
    }
} 