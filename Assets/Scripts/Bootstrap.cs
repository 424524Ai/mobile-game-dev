using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    void Awake()
    {
        Application.targetFrameRate = 60;
        DontDestroyOnLoad(gameObject);
    }
    private async Awaitable Start()
    {
        await SceneLoader.Instance.Load(
            "Menu",
            Application.exitCancellationToken
        );
    }
}
