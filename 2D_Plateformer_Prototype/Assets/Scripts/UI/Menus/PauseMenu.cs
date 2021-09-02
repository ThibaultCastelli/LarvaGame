using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    #region Variables
    [Header("COMPONENT")]
    [SerializeField] MenuManager menuManager;
    [Space]

    [Header("MENU")]
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject optionsMenu;
    [Space]

    [Header("EVENTS")]
    [SerializeField] Event killPlayerEvent;
    [SerializeField] EventInt loadSceneEvent;
    #endregion

    #region Starts & Updates
    private void Start()
    {
        // Subscribe to events
        menuManager.Controls.Player.Pause.started += Pause;
        menuManager.Controls.Player.Cancel.started += Cancel;
    }

    private void OnDisable()
    {
        // Unsubscribe to events
        menuManager.Controls.Player.Pause.started -= Pause;
        menuManager.Controls.Player.Cancel.started -= Cancel;
    }
    #endregion

    #region Functions
    void Pause(InputAction.CallbackContext ctx) => StartCoroutine(PauseCoroutine());

    void Cancel(InputAction.CallbackContext ctx)
    {
        if (mainMenu.activeInHierarchy)
            Continue();
    }

    public void Continue()
    {
        canvas.SetActive(false);
        GameManager.isGamePaused = false;
    }

    public void Retry()
    {
        Continue();
        killPlayerEvent.Raise();
    }

    public void SaveAndQuit()
    {
        loadSceneEvent.Raise(0);
    }
    #endregion

    #region Coroutines
    IEnumerator PauseCoroutine()
    {
        yield return null;
        // Pause
        if (GameManager.isGamePaused)
        {
            canvas.SetActive(true);

            // Select the first button
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(menuManager.FirstButtonSelected);
        }
        // Resume    
        else
        {
            canvas.SetActive(false);
            // Reset the menus to default state
            mainMenu.SetActive(true);
            optionsMenu.SetActive(false);
        }
    }
    #endregion
}
