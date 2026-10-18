using UnityEngine;

public class TrackGenerator : MonoBehaviour
{
    [SerializeField] private GameObject straightTrackPrefab;
    [SerializeField] private Transform lastTrack;
    [SerializeField] private int trackCount = 5;
    [SerializeField] private float trackLength = 60f;
    [SerializeField] private CornerTrigger cornerPrefab;
    [SerializeField] private RunnerMovement runner;

    private enum TrackDirection
    {
        Straight,
        Left,
        Right
    }

    private void Start()
    {
        // The centre of the next corner.
        Vector3 cornerPosition =
            lastTrack.position + lastTrack.forward * (trackLength / 2f);

        // Randomly choose left or right.
        bool turnRight = Random.value > 0.5f;

        float turnAngle = turnRight ? 90f : -90f;

        Quaternion newRotation =
            lastTrack.rotation * Quaternion.Euler(0f, turnAngle, 0f);

        Debug.Log("Generated Turn: " + (turnRight ? "Right" : "Left"));

        // Place the new track beyond the corner.
        float trackWidth = 8f;

        Vector3 spawnPosition =
            cornerPosition
            - lastTrack.forward * (trackWidth / 2f)
            + (newRotation * Vector3.forward) * (trackLength / 2f);

        GameObject newTrack = Instantiate(
            straightTrackPrefab,
            spawnPosition,
            newRotation
        );

        Vector3 adjustedCornerPosition =
        cornerPosition - lastTrack.forward * (trackWidth / 2f);

        CornerTrigger newCorner = Instantiate(
            cornerPrefab,
            adjustedCornerPosition,
            Quaternion.identity
        );

        newCorner.SetDirection(
            turnRight
                ? CornerTrigger.TurnDirection.Right
                : CornerTrigger.TurnDirection.Left
        );

        runner.RegisterCorner(newCorner);

        lastTrack = newTrack.transform;
    }
}
