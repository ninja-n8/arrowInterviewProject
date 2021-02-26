using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitioner : MonoBehaviour {
    public static event Action OnStartedLoading;
    public static event Action<float> OnProgressUpdated;
    public static event Action OnDone;

    private static SceneTransitioner instance;

    private static AsyncOperation operation;
    private static Coroutine routine = null;

    private static int previousSceneIndex;

    private void Awake () {
        if (!SceneManager.GetSceneByBuildIndex(0).isLoaded) {
            SceneManager.LoadScene(0);
        }
    }

    public static SceneTransitioner Instance {
        get {
            if (instance == null) {
                GameObject go = new GameObject();
                go.name = nameof(SceneTransitioner);
                instance = go.AddComponent<SceneTransitioner>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }

    public void LoadScene(int index) {
        if (routine != null) {
            Debug.LogWarning("Scene already loading.");
            return;
        }
        previousSceneIndex = SceneManager.GetActiveScene().buildIndex;
        Debug.LogWarning(previousSceneIndex);
        routine = StartCoroutine(LoadSceneAsync(index));
    }

    private static IEnumerator LoadSceneAsync(int index) {
        while (!SceneManager.GetSceneByBuildIndex(0).isLoaded) {
            yield return null;
        }

        var startTime = Time.realtimeSinceStartup;
        OnStartedLoading?.Invoke();
        operation = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
        operation.allowSceneActivation = false;
        var doneEventRaised = false;
        do {
            OnProgressUpdated?.Invoke(operation.progress);

            if (operation.progress >= 0.9f) {
                while (Time.realtimeSinceStartup - startTime < 5f) {
                    yield return null;
                }
                if (!doneEventRaised) {
                    OnDone?.Invoke();
                    doneEventRaised = true;
                }
            }
            yield return null;
        } while (!operation.isDone);

        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(index));
        Debug.LogWarning("Current: " + SceneManager.GetActiveScene().buildIndex + " Previous: " + previousSceneIndex);
        routine = null;
        operation = null;
    }

    public static void Finish() {
        operation.allowSceneActivation = true;
        if (SceneManager.sceneCount > 2) {
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneByBuildIndex(previousSceneIndex));
        }
        //SceneManager.UnloadSceneAsync(previousSceneIndex);
    }
}
