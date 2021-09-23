using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObserverTC;

public class OffsetTrigger : MonoBehaviour
{
    #region Variables
    [Header("CAMERA")]
    [SerializeField] [Range(-5f, 5f)] float newXOffset;
    [SerializeField] [Range(-5f, 5f)] float newYOffset;
    [SerializeField] [Range(1f, 10f)] float newOrtographicSize = 3.5f;
    [Space]

    [Header("EVENTS")]
    [SerializeField] NotifierVector2 changeOffsetEvent;
    [SerializeField] NotifierFloat changeOrtographicEvent;
    [Space]

    [Header("INFOS")]
    [SerializeField] [Range(0f, 4f)] float timeOfTransition = 1f;
    [SerializeField] bool showShape = true;

    Player player;

    float _previousXOffset;
    float _previousYOffset;
    float _previousOrtographicSize;

    bool _hasChange;
    bool _hasValues;
    bool _isOut;
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // Get all previous values
            if (!_hasValues)
            {
                player = collision.GetComponent<Player>();
                Cam cam = Camera.main.GetComponent<Cam>();

                _previousXOffset = cam.XOffset;
                _previousYOffset = cam.YOffset;
                _previousOrtographicSize = Camera.main.orthographicSize;

                _hasValues = true;
            }

            _isOut = false;
            StartCoroutine(TransitionCoroutine());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != "Player")
            return;

        _isOut = true;
    }

    IEnumerator TransitionCoroutine()
    {
        // Prevent from moving or shooting
        player.SetState(player.DeathState);

        // Set new or previous values
        if (_hasChange)
        {
            changeOffsetEvent.Notify(new Vector2(_previousXOffset, _previousYOffset));
            changeOrtographicEvent.Notify(_previousOrtographicSize);
        }
        else
        {
            changeOffsetEvent.Notify(new Vector2(newXOffset, newYOffset));
            changeOrtographicEvent.Notify(newOrtographicSize);
        }

        // Wait for the player to go out of the trigger
        while (!_isOut)
            yield return null;

        if (player.PreviousState == player.AirState)
            yield return new WaitForSeconds(timeOfTransition / 5);
        else
            yield return new WaitForSeconds(timeOfTransition);

        // Reset movement
        player.SetState(player.AirState);
        _hasChange = !_hasChange;
    }

    private void OnDrawGizmos()
    {
        if (showShape)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube(transform.position, transform.localScale);
        }
    }
}
