using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerOut : MonoBehaviour
{
    [SerializeField] EventInt loadSceneEvent;
    [SerializeField] bool showShape = true;

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
        if (collision.tag == "Player")
        {
            loadSceneEvent.Raise(SceneController.GetActiveScene() + 1);
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
