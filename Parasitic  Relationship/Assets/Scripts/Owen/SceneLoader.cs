using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* Current solution to load scenes
 * As we add more scenes, we'll want to unload / load a level one away from the current
 * This way the player can go forward or back a level without worry
 */
public static class SceneLoader
{
    static readonly int loadBuffer = 1;

    public static void SetScene(GameObject obj)
    {
        SetScene(obj.scene.buildIndex);
    }
    public static void SetScene(int buildIndex)
    {
        LoadNextScene(buildIndex);
        UnloadPreviousScene(buildIndex);
    }

    static void LoadNextScene(int buildIndex)
    {
        for(int buffer = 1; buffer <= loadBuffer; buffer++)
        {
            int nextScene = buildIndex + buffer;
            if (IndexInRange(nextScene) && !SceneAlreadyLoaded(nextScene))
            {
                Debug.Log($"Load Scene {nextScene}");
                LoadScene(nextScene);
            }

            int previousScene = buildIndex - buffer;
            if (IndexInRange(previousScene) && !SceneAlreadyLoaded(previousScene))
            {
                Debug.Log($"Load Scene {previousScene}");
                LoadScene(previousScene);
            }
        }
    }
    static void LoadScene(int buildIndex)
    {
        SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);
    }

    static void UnloadPreviousScene(int buildIndex)
    {
        int nextScene = buildIndex + loadBuffer + 1;
        if (IndexInRange(nextScene) && SceneAlreadyLoaded(nextScene))
        {
            Debug.Log($"Unload Scene {nextScene}");
            UnloadScene(nextScene);
        }

        int previousScene = buildIndex - loadBuffer - 1;
        if (IndexInRange(previousScene) && SceneAlreadyLoaded(previousScene))
        {
            Debug.Log($"Unload Scene {previousScene}");
            UnloadScene(previousScene);
        }
    }   
    static void UnloadScene(int buildIndex)
    {
        SceneManager.UnloadSceneAsync(buildIndex);
    }

    static bool IndexInRange(int buildIndex)
    {
        return 0 <= buildIndex && buildIndex < SceneManager.sceneCountInBuildSettings;
    }
    static bool SceneAlreadyLoaded(int buildIndex)
    {
        Scene scene = SceneManager.GetSceneByBuildIndex(buildIndex);
        return scene.isLoaded;
    }
}
