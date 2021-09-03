using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class SubMenu : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField] GameObject continueButton;
    [SerializeField] GameObject newGameButton;
    [SerializeField] EventSystem eventSystem;

    [Header("EVENTS")]
    [SerializeField] Event loadSaveEvent;
    [SerializeField] Event newGameEvent;

    private void OnEnable()
    {
        // Hide the "Continue" button if there is no save file
        if (!SaveManager.SaveExist())
        {
            continueButton.SetActive(false);
            StartCoroutine(SelectNewGameButton());
        }
    }

    public void NewGame()
    {
        newGameEvent.Raise();
    }

    public void ContinueGame()
    {
        loadSaveEvent.Raise();
    }

    IEnumerator SelectNewGameButton()
    {
        yield return null;
        // Select the first button
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(newGameButton);
    }
}
