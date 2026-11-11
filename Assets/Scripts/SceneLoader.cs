
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

// Loads scenes asynchronously and reports loading progress.
public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    [SerializeField] private GameObject loadingCanvas;
    [SerializeField] private UnityEngine.UI.Slider progressBar;

    private bool isLoading;

    private void Awake()
    {
        Instance = this;
    }

    public async Awaitable Load(string sceneName, CancellationToken ct)
    {
        if (isLoading)
            return;

        isLoading = true;

        try
        {
            if (loadingCanvas != null)
                loadingCanvas.SetActive(true);

            AsyncOperation operation =
                SceneManager.LoadSceneAsync(sceneName);

            operation.allowSceneActivation = false;

            while (operation.progress < 0.9f)
            {
                ct.ThrowIfCancellationRequested();

                if (progressBar != null)
                    progressBar.value = operation.progress / 0.9f;

                await Awaitable.NextFrameAsync(ct);
            }

            if (progressBar != null)
                progressBar.value = 1f;

            ct.ThrowIfCancellationRequested();

            operation.allowSceneActivation = true;
            await operation;
        }
        catch (OperationCanceledException)
        {
            Debug.Log("Scene loading cancelled.");
        }
        finally
        {
            if (loadingCanvas != null)
                loadingCanvas.SetActive(false);

            isLoading = false;
        }
    }
}
