using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ObserverTC;

public class TriggerOut : MonoBehaviour
{
    [SerializeField] NotifierInt loadSceneEvent;
    [SerializeField] Notifier killPlayerEvent;
    [SerializeField] bool showShape = true;

    bool _hasTriggerExit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Prevent from moving or shooting
        if (collision.tag == "Player")
        {

            Player player = collision.GetComponent<Player>();
            player.SetState(player.DeathState);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Load next level when player is out of screen
        if (collision.tag == "Player" && !_hasTriggerExit)
        {
            _hasTriggerExit = true;
            killPlayerEvent.Notify();
            loadSceneEvent.Notify(SceneController.GetActiveScene() + 1);
        }
    }

    private void OnDrawGizmos()
    {
        if (showShape)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, transform.localScale);
        }
    }
}
