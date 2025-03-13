using UnityEngine;

public class AIController : MonoBehaviour
{
    [Header("AI Settings")]
    public float moveSpeed = 4.5f;  // Slightly slower than player for fairness
    public float reactionDelay = 0.1f;  // Delay before reacting to ball movement
    public float predictionError = 0.2f;  // Random error in prediction to make AI beatable

    [Header("Boundaries")]
    public float topBoundary = 2.5f;
    public float bottomBoundary = -2.5f;

    [Header("References")]
    public BallController ball;

    private Rigidbody2D rb;
    private float targetY;
    private float lastDecisionTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Find ball if not assigned
        if (ball == null)
        {
            ball = FindObjectOfType<BallController>();
        }
    }

    private void Update()
    {
        // Only update if the game is in playing state
        if (GameManager.Instance.CurrentState != GameManager.GameState.Playing)
            return;

        // Only track ball if it's moving toward AI
        if (ball != null && IsBallMovingTowardAI())
        {
            // Update decision with delay
            if (Time.time - lastDecisionTime > reactionDelay)
            {
                DecideMovement();
                lastDecisionTime = Time.time;
            }
        }
        else
        {
            // Return to center when ball is moving away
            targetY = 0f;
        }

        // Move toward target position
        MoveTowardTarget();
    }

    private void DecideMovement()
    {
        // Predict where ball will be when it reaches AI's x position
        float ballVelocityX = ball.GetComponent<Rigidbody2D>().velocity.x;
        float ballVelocityY = ball.GetComponent<Rigidbody2D>().velocity.y;
        
        if (Mathf.Abs(ballVelocityX) > 0.1f)
        {
            float timeToReach = (transform.position.x - ball.transform.position.x) / ballVelocityX;
            
            if (timeToReach > 0)
            {
                // Predict ball position
                float predictedY = ball.transform.position.y + ballVelocityY * timeToReach;
                
                // Add random error to prediction
                predictedY += Random.Range(-predictionError, predictionError);
                
                // Clamp to boundaries
                predictedY = Mathf.Clamp(predictedY, bottomBoundary, topBoundary);
                
                // Set as target
                targetY = predictedY;
            }
        }
    }

    private void MoveTowardTarget()
    {
        // Calculate direction to move
        float direction = 0;
        
        if (Mathf.Abs(transform.position.y - targetY) > 0.1f)
        {
            direction = targetY > transform.position.y ? 1 : -1;
        }
        
        // Apply movement
        Vector2 movement = new Vector2(0, direction);
        Vector2 newPosition = rb.position + movement * moveSpeed * Time.deltaTime;
        
        // Clamp position within boundaries
        newPosition.y = Mathf.Clamp(newPosition.y, bottomBoundary, topBoundary);
        
        // Update position
        rb.MovePosition(newPosition);
    }

    private bool IsBallMovingTowardAI()
    {
        if (ball == null)
            return false;
            
        // Check if ball is moving toward AI side (right side)
        float ballVelocityX = ball.GetComponent<Rigidbody2D>().velocity.x;
        return ballVelocityX > 0;
    }
} 