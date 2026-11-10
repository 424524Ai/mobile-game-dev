using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;


public class RunnerMovement : MonoBehaviour
{
    [SerializeField] private float forwardSpeed = 6f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip turnSound;
    private readonly List<CornerTrigger> corners = new();
    private int turnIndex = 0;  // count turning 

    [SerializeField] private CornerTrigger[] initialCorners;
    [SerializeField] private RunnerMovement runner;

    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float jumpDuration = 0.8f;

    private float jumpTimer = 0f;
    private float groundY;
    private bool isJumping = false;

    // Update is called once per frame
    void Update()
    {
        if (LifecycleGuard.IsPaused) return;

        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

        UpdateJump();

        if (Keyboard.current == null) return;

        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            TryTurn(CornerTrigger.TurnDirection.Right);
        }

        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            TryTurn(CornerTrigger.TurnDirection.Left);
        }
    }

    private void Awake()
    {
        groundY = transform.position.y;

        foreach (CornerTrigger corner in initialCorners)
        {
            RegisterCorner(corner);
        }
    }

    public void TryTurn(CornerTrigger.TurnDirection direction)
    {
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

    public void Jump()
    {
        if (LifecycleGuard.IsPaused || isJumping) return;

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
}
