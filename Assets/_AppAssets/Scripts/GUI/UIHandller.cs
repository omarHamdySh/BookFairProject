using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private Slider loading;

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
    public void LoadLevel(string scencName)
    {
        StartCoroutine(LoadAsynchronously(scencName));
    }

    private IEnumerator LoadAsynchronously(string scencName)
    {
        yield return new WaitForSeconds(1f);
        AsyncOperation operation = SceneManager.LoadSceneAsync(scencName);
        while (!operation.isDone)
        {
            if (loading)
            {
                loading.value = operation.progress;
            }

            yield return null;
        }
    }
}
