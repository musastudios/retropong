using UnityEngine;

public class BallController : MonoBehaviour
{
    [Header("Ball Settings")]
    public float initialSpeed = 4f;
    public float speedIncrease = 0.5f;
    public float maxSpeed = 10f;
    
    [Header("Audio")]
    public AudioClip pingSound;
    public AudioClip pongSound;
    public AudioClip scoreSound;

    private Rigidbody2D rb;
    private AudioSource audioSource;
    private Vector2 direction;
    private float currentSpeed;
    private bool isMoving = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        
        // Initialize with zero velocity
        rb.velocity = Vector2.zero;
    }

    private void Start()
    {
        // Wait for game to start
        isMoving = false;
    }

    private void Update()
    {
        // Only update if the game is in playing state
        if (GameManager.Instance.CurrentState != GameManager.GameState.Playing)
            return;

        // Start moving if not already moving
        if (!isMoving)
        {
            StartMoving();
        }
    }

    public void ResetBall()
    {
        // Reset position to center
        transform.position = Vector3.zero;
        
        // Stop movement
        rb.velocity = Vector2.zero;
        isMoving = false;
        
        // Reset speed
        currentSpeed = initialSpeed;
        
        // Wait a moment before starting again
        Invoke("StartMoving", 1.0f);
    }

    private void StartMoving()
    {
        // Choose a random direction (left or right)
        float randomX = Random.value > 0.5f ? 1f : -1f;
        
        // Choose a random y direction
        float randomY = Random.Range(-0.5f, 0.5f);
        
        // Normalize and set direction
        direction = new Vector2(randomX, randomY).normalized;
        
        // Apply velocity
        rb.velocity = direction * currentSpeed;
        
        isMoving = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Handle paddle collisions
        if (collision.gameObject.CompareTag("Paddle"))
        {
            HandlePaddleCollision(collision);
        }
        // Handle wall collisions
        else if (collision.gameObject.CompareTag("Wall"))
        {
            // Play bounce sound
            PlaySound(pingSound);
        }
    }

    private void HandlePaddleCollision(Collision2D collision)
    {
        // Increase speed
        currentSpeed = Mathf.Min(currentSpeed + speedIncrease, maxSpeed);
        
        // Calculate reflection angle based on where the ball hit the paddle
        PlayerController paddle = collision.gameObject.GetComponent<PlayerController>();
        if (paddle != null)
        {
            // Get the hit position relative to the paddle center
            float hitPos = (transform.position.y - collision.transform.position.y) / collision.collider.bounds.size.y;
            
            // Calculate new direction with some angle variation based on hit position
            float bounceAngle = hitPos * 60f; // Max 60 degree bounce
            
            // Determine if ball is moving left or right
            float directionX = transform.position.x > 0 ? -1f : 1f;
            
            // Calculate new direction
            direction = new Vector2(directionX, Mathf.Sin(bounceAngle * Mathf.Deg2Rad)).normalized;
            
            // Apply new velocity
            rb.velocity = direction * currentSpeed;
            
            // Play paddle hit sound
            PlaySound(pongSound);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if ball entered a goal trigger
        if (collision.CompareTag("Goal1"))
        {
            // Player 2 scored
            GameManager.Instance.PlayerScored(2);
            PlaySound(scoreSound);
        }
        else if (collision.CompareTag("Goal2"))
        {
            // Player 1 scored
            GameManager.Instance.PlayerScored(1);
            PlaySound(scoreSound);
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
} 