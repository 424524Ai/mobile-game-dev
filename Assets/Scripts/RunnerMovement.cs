using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
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
    [SerializeField] private AudioClip turnSound;
    private readonly List<CornerTrigger> corners = new();
    private int turnIndex = 0;  // count turning 
    private bool isGameOver = false;

    [SerializeField] private CornerTrigger[] initialCorners;

    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float jumpDuration = 0.8f;

    private float jumpTimer = 0f;
    private float groundY;
    private bool isJumping = false;

    [SerializeField] private GameManager gameManager;

    // Update is called once per frame
    void Update()
    {
        if (LifecycleGuard.IsPaused || isGameOver) return;

        UpdateSpeed();

        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

        CheckMissedTurn();

        UpdateJump();

        if (Keyboard.current == null) return;

        if (Keyboard.current != null)
        {
            // Turn right using the right arrow key.
            if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
            {
                TryTurn(CornerTrigger.TurnDirection.Right);
            }
            // Turn left using the left arrow key.
            if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
            {
                TryTurn(CornerTrigger.TurnDirection.Left);
            }

            // Jump using the space key.
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
                Jump();
        }
        UpdateScore();
    }

    private void Awake()
    {
        groundY = transform.position.y;

        foreach (CornerTrigger corner in initialCorners)
        {
            RegisterCorner(corner);
        }

        previousPosition = transform.position;

        GameSession.FinalScore = 0;
        UpdateScoreText();
    }

    public void TryTurn(CornerTrigger.TurnDirection direction)
    {
        if (LifecycleGuard.IsPaused || isGameOver) return;

        if (turnIndex >= corners.Count) return;

        CornerTrigger corner = corners[turnIndex];

        if (corner == null || corner.Direction != direction) return;

        Vector3 cornerPos = corner.transform.position;

        // Distance along the runner's current forward direction.
        float distance = Vector3.Dot(
            cornerPos - transform.position,
            transform.forward);

        if (Mathf.Abs(distance) <= corner.TurnTolerance)
        {
            // Align the runner with corner centre.
            Vector3 pos = transform.position;
            pos.x = cornerPos.x;
            pos.z = cornerPos.z;
            transform.position = pos;

            float angle = direction == CornerTrigger.TurnDirection.Right ? 90f : -90f;

            transform.Rotate(0f, angle, 0f);

            turnIndex++;

            // Remove the completed corner.
            CleanupPassedCorners();

            if (audioSource != null && turnSound != null)
            {
                audioSource.PlayOneShot(turnSound);
            }

            Debug.Log("Turn completed: " + direction);
        }
        else
        {
            Debug.Log("Too early or too late to turn");
        }
    }

    public void RegisterCorner(CornerTrigger corner)
    {
        if (corner != null && !corners.Contains(corner))
        {
            corners.Add(corner);
        }
    }

    // Remove corners that the runner has already passed.
    private void CleanupPassedCorners()
    {
        while (turnIndex > 0)
        {
            CornerTrigger passedCorner = corners[0];

            corners.RemoveAt(0);
            turnIndex--;

            if (passedCorner != null)
            {
                Destroy(passedCorner.gameObject);
            }
        }
    }


    public void Jump()
    {
        if (LifecycleGuard.IsPaused || isGameOver || isJumping) return;

        isJumping = true;
        jumpTimer = 0f;
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

    private void CheckMissedTurn()
    {
        if (turnIndex >= corners.Count) return;

        CornerTrigger corner = corners[turnIndex];

        if (corner == null) return;

        // Calculate distance along the runner's forward direction.
        float distance = Vector3.Dot(
            corner.transform.position - transform.position,
            transform.forward
        );

        // The runner has passed the allowed turning area.
        if (distance < -corner.TurnTolerance)
        {
            isGameOver = true;

            // Vibrate once when the runner misses a turn.
            Haptics.Pulse();

            Debug.Log("Game Over - Missed Turn");

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
        if (scoreText != null)
        {
            scoreText.text = $"Score: {GameSession.FinalScore}";
        }
    }
}
