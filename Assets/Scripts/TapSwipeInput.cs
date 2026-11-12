
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class TapSwipeInput : MonoBehaviour
{
    [SerializeField] private RunnerMovement runner;

    public float swipeDp = 50f;     // Distance in dp, not pixels
    public float tapMax = 0.3f;     // Seconds

    void OnEnable() => EnhancedTouchSupport.Enable();
    void OnDisable() => EnhancedTouchSupport.Disable();

    void Update()
    {
        if (LifecycleGuard.IsPaused) return;

        foreach (var t in Touch.activeTouches)
        {
            if (t.phase != TouchPhase.Ended) continue;

            float px = swipeDp * Mathf.Max(Screen.dpi, 160f) / 160f;
            Vector2 d = t.screenPosition - t.startScreenPosition;

            if (d.magnitude >= px)
            {
                if (Mathf.Abs(d.y) > Mathf.Abs(d.x))
                {
                    // Vertical swipe
                    if (d.y > 0)
                    {
                        Debug.Log("Swipe Up - Jump");
                        if (runner != null) runner.Jump();
                    }
                    else
                    {
                        Debug.Log("Swipe Down - Slide");
                        if (runner != null) runner.Slide();
                    }
                }
                else
                {
                    // Horizontal swipe
                    if (d.x > 0)
                    {
                        Debug.Log("Swipe Right - Change Lane");

                        if (runner != null)
                        {
                            runner.ChangeLane(1);
                        }
                    }
                    else
                    {
                        Debug.Log("Swipe Left - Change Lane");

                        if (runner != null)
                        {
                            runner.ChangeLane(-1);
                        }
                    }
                }
            }
            else if (t.time - t.startTime < tapMax)
            {
                Debug.Log("Tap");
            }
        }
    }
}
