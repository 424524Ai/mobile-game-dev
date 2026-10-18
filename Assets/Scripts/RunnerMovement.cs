using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;


public class RunnerMovement : MonoBehaviour
{
    [SerializeField] private float forwardSpeed = 6f;
    private readonly List<CornerTrigger> corners = new();
    private int turnIndex = 0;  // count turning 

    [SerializeField] private CornerTrigger[] initialCorners;

    // Update is called once per frame
    void Update()
    {
        if (LifecycleGuard.IsPaused) return;

        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

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
        foreach (CornerTrigger corner in initialCorners)
        {
            RegisterCorner(corner);
        }
    }

    private void TryTurn(CornerTrigger.TurnDirection direction)
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
}
