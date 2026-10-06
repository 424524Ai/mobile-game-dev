using UnityEngine;

public class RunnerMovement : MonoBehaviour
{
    [SerializeField] private float forwardSpeed = 6f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);
    }
}
