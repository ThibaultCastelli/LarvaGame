using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] PlayerInput input;

    [Space]
    [SerializeField] EventBool flipSpriteEvent;

    public bool FlipX { get { return sprite.flipX; } }

    Vector2 _gunDirection;
    float _movementDir;

    private void Update()
    {
        _gunDirection = input.GetGunDirection();
        _movementDir = input.GetMovementX();

        if (_movementDir < 0 && !sprite.flipX)
        {
            sprite.flipX = true;
            flipSpriteEvent.Raise(true);
        }
        else if (_movementDir > 0 && sprite.flipX)
        {
            sprite.flipX = false;
            flipSpriteEvent.Raise(false);
        }
    }
}
