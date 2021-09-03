using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    #region Variables
    [Header("TRANSITIONS")]
    [SerializeField] Animator transitionLevelAnimator;
    [SerializeField] Animator transitionDeathAnimator;
    [SerializeField] [Range(0f, 2f)] float transitionTime = 1f;

    [Header("INFOS")]
    [SerializeField] SaveManager save;
    #endregion

    private void Start()
    {
        // Select wich transition to play
        if (save == null || save.currentSave.isNewLevel || GetActiveScene() == 0)
            transitionLevelAnimator.SetTrigger("End_Transition");
        else
            transitionDeathAnimator.SetTrigger("End_Transition");
    }

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
        // Select wich transition to play
        if (sceneIndex == SceneManager.GetActiveScene().buildIndex)
            transitionDeathAnimator.SetTrigger("Start_Transition");
        else
            transitionLevelAnimator.SetTrigger("Start_Transition");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneIndex);
    }
    #endregion
}
