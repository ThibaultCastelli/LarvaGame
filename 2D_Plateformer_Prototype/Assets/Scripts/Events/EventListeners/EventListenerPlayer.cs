using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventListenerPlayer : MonoBehaviour
{
    public EventPlayer gameEvent;
    public UnityEvent<Player> responsePlayer;

    private void OnEnable()
    {
        gameEvent.SubscribeEvent(this);
    }

    private void OnDisable()
    {
        gameEvent.UnsubscribeEvent(this);
    }

    public void OnEventRaise(Player player)
    {
        responsePlayer?.Invoke(player);
    }
}
