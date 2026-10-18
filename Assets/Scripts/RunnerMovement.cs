using UnityEngine;
using UnityEngine.InputSystem;

public class RunnerMovement : MonoBehaviour
{
    [SerializeField] private float forwardSpeed = 6f;
    [SerializeField] private float turnZ = 56f;     // right turning point on Z axis
    [SerializeField] private float turnTolerance = 3f;  // accept early/late turning distance
    [SerializeField] private float secondTurnX = 56f; // left turning point on X axis
    private int turnIndex = 0;  // count turning 

    // Update is called once per frame
    void Update()
    {
        if (LifecycleGuard.IsPaused) return;

        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

        if (Keyboard.current != null &&
            Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            TurnRight();
        }

        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            TurnLeft();
        }
    }

    public void TurnRight()
    {
        if (turnIndex != 0)
        {
            return;
        }
        if (Mathf.Abs(transform.position.z - turnZ) <= turnTolerance)
        {
            // Align the runner with the centre of the second track
            Vector3 pos = transform.position;
            pos.z = turnZ;
            transform.position = pos;

            // Turn right by 90 degrees
            transform.Rotate(0f, 90f, 0f);
            turnIndex = 1;
            Debug.Log("First turn completed: Right");
        }
        else
        {
            Debug.Log("Too early or too late to turn");
        }
    }

    public void TurnLeft() {
        if (turnIndex != 1)
        {
            return;
        }
        if (Mathf.Abs(transform.position.x - secondTurnX) <= turnTolerance){
            Vector3 pos = transform.position;
            pos.x = secondTurnX;
            transform.position = pos;

            transform.Rotate(0f, -90f, 0f);
            turnIndex = 2;
            Debug.Log("Second turn completed: Left");
        }
        else
        {
            Debug.Log("Too early or too late to turn left");
        }
    }
}
