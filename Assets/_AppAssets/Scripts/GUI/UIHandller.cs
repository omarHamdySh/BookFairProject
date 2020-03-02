using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class UIHandller : MonoBehaviour
{
    [Header("LoadingBar")]
    [SerializeField] private Slider endedLoadingBar;

    /// <summary>
    /// Exit the game
    /// </summary>
    public void QuitTheGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Load the MainMenu Scene
    /// </summary>
    public void BackToMainMenu()
    {
        StartCoroutine(LoadAsynchronously("MainMenu"));
    }

    /// <summary>
    /// Restart the current Scene
    /// </summary>
    public void RestartLevel()
    {
        StartCoroutine(LoadAsynchronously(SceneManager.GetActiveScene().name));
    }

    /// <summary>
    /// Moving from scene to another
    /// </summary>
    /// <param name="scencName">index of the scene you will go to</param>
    public void LoadLevel(string scencName, bool loadImmediately = false)
    {
        if (!loadImmediately)
            StartCoroutine(LoadAsynchronously(scencName));
        else
        {
            SceneManager.LoadScene(scencName);
        }
    }


    private IEnumerator LoadAsynchronously(string scencName)
    {
        yield return new WaitForSeconds(1f);
        AsyncOperation operation = SceneManager.LoadSceneAsync(scencName);
        while (!operation.isDone)
        {
            if (endedLoadingBar)
            {
                endedLoadingBar.value = operation.progress;
            }

            yield return null;
        }
    }
}
