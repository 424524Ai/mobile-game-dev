using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class RunnerMovement : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    private float totalScore = 0f;
    private Vector3 previousPosition;

    [SerializeField] private float forwardSpeed = 6f;
    [SerializeField] private float speedIncreaseInterval = 15f;
    [SerializeField] private float speedIncreaseAmount = 1f;
    [SerializeField] private float maxSpeed = 12f;

    private float speedTimer = 0f;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip jumpSound;
    private bool isGameOver = false;


    [Header("Lane Movement")]
    [SerializeField] private float laneWidth = 2.4f;
    [SerializeField] private float laneChangeSpeed = 10f;

    private int currentLane = 1; // 0 = Left, 1 = Center, 2 = Right
    private float centerX;

    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float jumpDuration = 0.8f;

    private float jumpTimer = 0f;
    private float groundY;
    private bool isJumping = false;

    [SerializeField] private GameManager gameManager;

    private int lastDisplayedScore = -1;

    [Header("Roll Settings")]
    [SerializeField] private float rollDuration = 0.8f;
    [SerializeField] private float rollHeight = 0.9f;
    private bool isRolling = false;
    private float rollTimer = 0f;

    [SerializeField] private CapsuleCollider capsuleCollider;
    [SerializeField] private Animator animator;
    
    private float originalColliderHeight;
    private Vector3 originalColliderCenter;
    

    // Update is called once per frame
    void Update()
    {
        if (LifecycleGuard.IsPaused || isGameOver) return;

        UpdateSpeed();

        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

        // Smoothly move towards the selected lane.
        UpdateLaneMovement();

        // Keep the existing jump mechanic.
        UpdateJump();

        UpdateRoll();

        UpdateScore();

        if (Keyboard.current == null) return;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
            {
                ChangeLane(-1);
            }

            if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
            {
                ChangeLane(1);
            }

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                Jump();
            }

            if (Keyboard.current.downArrowKey.wasPressedThisFrame)
            {
                Roll();
            }
        }
    }

    public void ChangeLane(int direction)
    {
        if (LifecycleGuard.IsPaused || isGameOver) return;

        // Keep the lane index between 0 and 2.
        currentLane = Mathf.Clamp(currentLane + direction, 0, 2);
    }

    private void UpdateLaneMovement()
    {
        // Calculate the target X position.
        float targetX = centerX + (currentLane - 1) * laneWidth;

        Vector3 position = transform.position;

        // Move smoothly towards the target lane.
        position.x = Mathf.MoveTowards(
            position.x,
            targetX,
            laneChangeSpeed * Time.deltaTime
        );

        transform.position = position;
    }

    private void Awake()
    {
        originalColliderHeight = capsuleCollider.height;
        originalColliderCenter = capsuleCollider.center;

        groundY = transform.position.y;
        centerX = transform.position.x;

        previousPosition = transform.position;

        GameSession.FinalScore = 0;
        UpdateScoreText();
    }

   
    public void Jump()
    {
        if (LifecycleGuard.IsPaused || isGameOver || isJumping || isRolling) return;

        if (audioSource != null && jumpSound != null)
        {
            audioSource.PlayOneShot(jumpSound);
        }

        isJumping = true;
        jumpTimer = 0f;

        // Play the roll animation.
        if (animator != null)
        {
            animator.SetTrigger("Jump");
        }
    }

    private void UpdateJump()
    {
        if (!isJumping) return;

        jumpTimer += Time.deltaTime;

        float progress = Mathf.Clamp01(jumpTimer / jumpDuration);

        float height = 4f * jumpHeight * progress * (1f - progress);

        Vector3 position = transform.position;
        position.y = groundY + height;
        transform.position = position;

        if (progress >= 1f)
        {
            isJumping = false;

            position.y = groundY;
            transform.position = position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isGameOver) return;

        if (other.CompareTag("Obstacle"))
        {
            isGameOver = true;

            // Vibrate once when the runner hits an obstacle.
            Haptics.Pulse();

            Debug.Log("Game Over - Hit Obstacle");

            if (gameManager != null)
            {
                gameManager.SetState(GameManager.State.Lost);
            }
            else
            {
                Debug.LogError("GameManager is not assigned!");
            }

            enabled = false;
        }
    }

    private void UpdateSpeed()
    {
        if (forwardSpeed >= maxSpeed) return;

        speedTimer += Time.deltaTime;

        if (speedTimer >= speedIncreaseInterval)
        {
            speedTimer -= speedIncreaseInterval;

            forwardSpeed = Mathf.Min(
                forwardSpeed + speedIncreaseAmount,
                maxSpeed
            );

            Debug.Log("Runner Speed: " + forwardSpeed);
        }
    }

    // Calculate the total horizontal distance travelled.
    private void UpdateScore()
    {
        Vector3 currentPosition = transform.position;

        Vector3 movement = currentPosition - previousPosition;
        movement.y = 0f;

        totalScore += movement.magnitude;

        previousPosition = currentPosition;

        GameSession.FinalScore = Mathf.FloorToInt(totalScore);

        UpdateScoreText();
    }

    // Display the current distance on the HUD.
    private void UpdateScoreText()
    {
        if (scoreText == null)
            return;

        int currentScore = GameSession.FinalScore;

        if (currentScore == lastDisplayedScore)
            return;

        scoreText.SetText("Score: {0}", currentScore);
        lastDisplayedScore = currentScore;
    }

    public void Roll()
    {
        if (LifecycleGuard.IsPaused || isGameOver || isRolling || isJumping)
            return;

        isRolling = true;
        rollTimer = 0f;

        // Reduce collider height while keeping its bottom position.
        capsuleCollider.height = rollHeight;

        Vector3 center = originalColliderCenter;
        center.y -= (originalColliderHeight - rollHeight) / 2f;
        capsuleCollider.center = center;

        // Play the roll animation.
        if (animator != null)
        {
            animator.SetTrigger("Roll");
        }
    }

    private void UpdateRoll()
    {
        if (!isRolling)
            return;

        rollTimer += Time.deltaTime;

        if (rollTimer >= rollDuration)
        {
            isRolling = false;

            // Restore the original collider.
            capsuleCollider.height = originalColliderHeight;
            capsuleCollider.center = originalColliderCenter;
        }
    }
}
