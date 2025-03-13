using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    public int playerNumber = 1;
    public float moveSpeed = 5f;
    public KeyCode upKey = KeyCode.W;
    public KeyCode downKey = KeyCode.S;

    [Header("Boundaries")]
    public float topBoundary = 2.5f;
    public float bottomBoundary = -2.5f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Set default controls based on player number
        if (playerNumber == 1)
        {
            upKey = KeyCode.W;
            downKey = KeyCode.S;
        }
        else if (playerNumber == 2)
        {
            upKey = KeyCode.UpArrow;
            downKey = KeyCode.DownArrow;
        }
    }

    private void Update()
    {
        // Only process input if the game is in playing state
        if (GameManager.Instance.CurrentState != GameManager.GameState.Playing)
            return;

        HandleInput();
    }

    private void HandleInput()
    {
        Vector2 movement = Vector2.zero;

        // Check for key presses
        if (Input.GetKey(upKey))
        {
            movement.y = 1f;
        }
        else if (Input.GetKey(downKey))
        {
            movement.y = -1f;
        }

        // Apply movement
        if (movement != Vector2.zero)
        {
            // Calculate new position
            Vector2 newPosition = rb.position + movement * moveSpeed * Time.deltaTime;
            
            // Clamp position within boundaries
            newPosition.y = Mathf.Clamp(newPosition.y, bottomBoundary, topBoundary);
            
            // Update position
            rb.MovePosition(newPosition);
        }
    }
} 