using UnityEngine;

public class CornerTrigger : MonoBehaviour
{
    public enum TurnDirection
    {
        Left,
        Right
    }

    [SerializeField] private TurnDirection direction;
    [SerializeField] private float turnTolerance = 5f;

    public TurnDirection Direction => direction;
    public float TurnTolerance => turnTolerance;

    public void SetDirection(TurnDirection newDirection)
    {
        direction = newDirection;
    }
}