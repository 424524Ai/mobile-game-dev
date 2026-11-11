
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
        AsyncOperation operation = null;

        try
        {
            ct.ThrowIfCancellationRequested();

            if (progressBar != null)
                progressBar.value = 1f;

            if (loadingCanvas != null)
                loadingCanvas.SetActive(true);

            operation = SceneManager.LoadSceneAsync(sceneName);

            operation.allowSceneActivation = false;

            while (operation.progress < 0.9f)
            {
                ct.ThrowIfCancellationRequested();

                if (progressBar != null)
                    progressBar.value = operation.progress / 0.9f;

                await Awaitable.NextFrameAsync(ct);
            }

            ct.ThrowIfCancellationRequested();

            if (progressBar != null)
                progressBar.value = 1f;

            operation.allowSceneActivation = true;
            await operation;
        }
        catch (OperationCanceledException)
        {
            Debug.Log("Scene loading cancelled.");

            // Release the pending scene activation to avoid blocking Unity.
            if (operation != null && !operation.isDone)
            {
                operation.allowSceneActivation = true;
                await operation;
            }
        }
        finally
        {
            if (loadingCanvas != null)
                loadingCanvas.SetActive(false);

            isLoading = false;
        }
    }
}
