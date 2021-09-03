using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    #region Variables
    [Header("MENU")]
    [SerializeField] GameObject baseMenu;
    [SerializeField] EventSystem eventSystem;
    [SerializeField] GameObject firstButtonSelected;
    [Space]

    [Header("COLOR")]
    [SerializeField] Color defaultTxtColor;
    [SerializeField] Color selectedTxtColor;
    [SerializeField] TextMeshProUGUI[] texts;
    [Space]

    // Menu Infos
    GameObject _currentMenu;
    GameObject _previousMenu;
    GameObject _previousFirstButtonSelected;

    // Event System
    BaseEventData _eventData;
    GameObject _previousSelected = null;

    // Text Mesh Pro
    TextMeshProUGUI _selectedTxt;
    TextMeshProUGUI _previousTxtSelected = null;

    public PlayerControls Controls { get; private set; }
    #endregion

    #region Properties
    public GameObject FirstButtonSelected { get { return firstButtonSelected; } }
    #endregion

    #region Starts & Updates
    private void Awake()
    {
        // Enable the input controls
        Controls = new PlayerControls();
        Controls.Player.Enable();
    }
    private void OnEnable()
    {
        // Subscribe to events
        Controls.Player.Cancel.started += Cancel;
    }

    private void OnDisable()
    {
        // Unsubscribe to events
        Controls.Player.Cancel.started -= Cancel;
    }

    private void Start()
    {
        // Initialize variables
        _eventData = new BaseEventData(eventSystem);
        _currentMenu = baseMenu;

        // Set default text color to each buttons
        foreach (TextMeshProUGUI text in texts)
            text.color = defaultTxtColor;

        // Select the first button
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButtonSelected);
    }

    private void Update()
    {
        // Change the color of the selected text
        if (_eventData.selectedObject != _previousSelected)
            ChangeTxtColor();

        _previousSelected = _eventData.selectedObject;
    }
    #endregion

    #region Functions
    void Cancel(InputAction.CallbackContext ctx) => StartCoroutine(CancelCoroutine());

    public void GoToMenu ()
    {
        // Get the infos with NextMenuInfos script attached to the button
        NextMenuInfos menuInfos = EventSystem.current.currentSelectedGameObject.GetComponent<NextMenuInfos>();
        menuInfos.previousMenu.SetActive(false);
        menuInfos.nextMenu.SetActive(true);

        // Select the first button
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(menuInfos.nextFirstButtonSelected);

        // Update values
        _currentMenu = menuInfos.nextMenu;
        _previousMenu = menuInfos.previousMenu;
        _previousFirstButtonSelected = menuInfos.previousFirstButtonSelected;
    }

    void ChangeTxtColor()
    {
        // Find the txt of the button and if not null, change its color
        _selectedTxt = _eventData.selectedObject.GetComponentInChildren<TextMeshProUGUI>();

        if (_selectedTxt != null)
            _selectedTxt.color = selectedTxtColor;

        // Reset the color of the previous txt
        if (_previousTxtSelected != null)
            _previousTxtSelected.color = defaultTxtColor;

        _previousTxtSelected = _selectedTxt;
    }
    #endregion

    #region Couroutines
    IEnumerator CancelCoroutine()
    {
        yield return null;

        // Only go to the previous menu if not on the base menu
        if (_currentMenu == baseMenu)
            yield break;

        _currentMenu.SetActive(false);
        _previousMenu.SetActive(true);

        // Select the first button
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_previousFirstButtonSelected);

        _currentMenu = _previousMenu;
    }
    #endregion
}
