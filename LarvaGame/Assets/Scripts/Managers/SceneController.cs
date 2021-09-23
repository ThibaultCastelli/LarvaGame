using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using EasingTC;
using ObserverTC;

public class SceneController : MonoBehaviour
{
    #region Variables
    [Header("TRANSITIONS")]
    [SerializeField] [Range(0f, 2f)] float transitionTime = 1f;
    #endregion


    #region Functions
    public void ReloadScene()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
    }

    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(LoadLevel(sceneIndex));
    }

    public static int GetActiveScene()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public static int GetCountScene()
    {
        return SceneManager.sceneCountInBuildSettings;
    }
    #endregion

    #region Coroutines
    IEnumerator LoadLevel(int sceneIndex)
    {
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneIndex);
    }
    #endregion
}
