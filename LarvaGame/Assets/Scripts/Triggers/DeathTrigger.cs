using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObserverTC;

public class DeathTrigger : MonoBehaviour
{
    [SerializeField] Notifier deathEvent;
    [SerializeField] bool showShape;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            deathEvent.Notify();
    }

    private void OnDrawGizmos()
    {
        if (showShape)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, transform.localScale);
        }
    }
}
