using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObserverTC;

public class Collectible : MonoBehaviour
{
    #region Variables
    [SerializeField] [Range(0.01f, 0.2f)] float speedFollow = 0.05f;
    [SerializeField] [Range(0f, 4f)] float timeToCollect = 1f;
    [SerializeField] Notifier larvaCollectedEvent;

    bool _isCollected = false;
    bool _isInTrigger = false;
    Transform _target;
    Player _player;
    #endregion

    #region Starts & Updates
    private void FixedUpdate()
    {
        if (_isCollected && !GameManager.isGamePaused)
        {
            // Follow the player
            transform.position = Vector2.Lerp(transform.position, _target.position, speedFollow);

            // Collect only if on ground
            if (_player.CurrentState == _player.GroundState && !_isInTrigger)
                StartCoroutine(GetCollectible());
            else
                StopAllCoroutines();
        }

        if (GameManager.isGamePaused)
            StopAllCoroutines();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _target = collision.transform;
            _player = _target.GetComponent<Player>();
            _isCollected = true;
        }
        if (collision.tag == "CollectibleTrigger")
            _isInTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "CollectibleTrigger")
            _isInTrigger = false;
    }
    #endregion

    #region Coroutines
    IEnumerator GetCollectible()
    {
        // Add the collectible once player spend some times on the ground and save
        yield return new WaitForSeconds(timeToCollect);
        larvaCollectedEvent.Notify();
        Destroy(this.gameObject);
    }
    #endregion
}
