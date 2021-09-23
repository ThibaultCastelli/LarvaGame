using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObserverTC;

public class TriggerZone : MonoBehaviour
{
    #region Variables
    [Header("CAMERA")]
    [SerializeField] [Range(-5f, 5f)] float newXOffset;
    [SerializeField] [Range(-5f, 5f)] float newYOffset;
    [SerializeField] [Range(1f, 10f)] float newOrthographicSize = 3.5f;
    [Space]
    [SerializeField] float leftBound;
    [SerializeField] float rightBound;
    [SerializeField] float topBound;
    [SerializeField] float bottomBound;
    [Space]

    [Header("EVENTS")]
    [SerializeField] NotifierVector2 changeOffsetEvent;
    [SerializeField] NotifierFloat changeOrthographicEvent;
    [SerializeField] NotifierFloat4 changeBoundariesEvent;
    [SerializeField] NotifierVector2 savePosEvent;
    [SerializeField] NotifierVector2 saveCamPosEvent;
    [SerializeField] Notifier saveCheckpointEvent;
    [Space]

    [Header("INFOS")]
    [SerializeField] TriggerZone previousTrigger;
    [SerializeField] [Range(0f, 4f)] float timeOfTransition = 1f;
    [SerializeField] bool showShape = true;
    [SerializeField] bool showBoundaries = true;

    Player player;

    float _previousXOffset;
    float _previousYOffset;
    float _previousOrtographicSize;
    float _previousLeftBound;
    float _previousRightBound;
    float _previousTopBound;
    float _previousBottomBound;

    bool _hasChange;
    bool _isOut;
    bool _isInTransition;
    #endregion

    #region Getters
    public float xOffset { get { return newXOffset; } }
    public float yOffset { get { return newYOffset; } }
    public float orthographicSize { get { return newOrthographicSize; } }
    public float left { get { return leftBound; } }
    public float right { get { return rightBound; } }
    public float top { get { return topBound; } }
    public float bot { get { return bottomBound; } }
    #endregion

    private void Start()
    {
        // Get values and check if the player is before or after the trigger
        Cam cam = Camera.main.GetComponent<Cam>();
        if (cam.LeftBound == leftBound)
            _hasChange = true;

        _previousXOffset = previousTrigger.xOffset;
        _previousYOffset = previousTrigger.yOffset;
        _previousOrtographicSize = previousTrigger.orthographicSize;
        _previousLeftBound = previousTrigger.left;
        _previousRightBound = previousTrigger.right;
        _previousTopBound = previousTrigger.top;
        _previousBottomBound = previousTrigger.bot;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player" || _isInTransition)
            return;

        _isOut = false;
        player = collision.GetComponent<Player>();
        StartCoroutine(TransitionCoroutine());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != "Player")
            return;

        _isOut = true;
    }

    IEnumerator TransitionCoroutine()
    {
        _isInTransition = true;

        // Prevent from moving or shooting
        player.SetState(player.DeathState);

        // Set new or previous values
        if (!_hasChange)
        {
            changeOffsetEvent.Notify(new Vector2(newXOffset, newYOffset));
            changeOrthographicEvent.Notify(newOrthographicSize);
            changeBoundariesEvent.Notify(leftBound, rightBound, topBound, bottomBound);
        }
        else
        {
            changeOffsetEvent.Notify(new Vector2(_previousXOffset, _previousYOffset));
            changeOrthographicEvent.Notify(_previousOrtographicSize);
            changeBoundariesEvent.Notify(_previousLeftBound, _previousRightBound, _previousTopBound, _previousBottomBound);
        }

        // Wait for the player to go out of the trigger
        while (!_isOut)
            yield return null;

        if (player.PreviousState == player.AirState)
            yield return new WaitForSeconds(timeOfTransition / 5);
        else
            yield return new WaitForSeconds(timeOfTransition);

        // Reset movement and save positions
        player.SetState(player.AirState);
        savePosEvent.Notify(new Vector2(player.transform.position.x, player.transform.position.y));
        saveCamPosEvent.Notify(new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y));
        saveCheckpointEvent.Notify();

        _hasChange = !_hasChange;
        _isInTransition = false;
    }

    private void OnDrawGizmos()
    {
        if (showShape)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position, transform.localScale);
        }

        if (showBoundaries)
        {
            float _halfHeight = newOrthographicSize;
            float _halfWidth = _halfHeight * Camera.main.aspect;

            Gizmos.color = Color.red;

            Vector3 topLeft = new Vector3(leftBound - _halfWidth, topBound + _halfHeight);
            Vector3 topRight = new Vector3(rightBound + _halfWidth, topBound + _halfHeight);
            Vector3 bottomLeft = new Vector3(leftBound - _halfWidth, bottomBound - _halfHeight);
            Vector3 bottomRight = new Vector3(rightBound + _halfWidth, bottomBound - _halfHeight);

            Gizmos.DrawLine(topLeft, bottomLeft);
            Gizmos.DrawLine(topRight, bottomRight);
            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(bottomLeft, bottomRight);
        }
    }
}
