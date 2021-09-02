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

    private void Update()
    {
        _gunDirection = input.GetGunDirection();

        if (player.IsAgainstWallLeft)
            flipSpriteEvent.Raise(false);
        else if (player.IsAgainstWallRight)
            flipSpriteEvent.Raise(true);
        else
        {
            if (_gunDirection.x < -0.5f && !sprite.flipX)
                flipSpriteEvent.Raise(true);
            else if (_gunDirection.x > 0.5f && sprite.flipX)
                flipSpriteEvent.Raise(false);
        }
    }

    public void FlipSprite(bool flip) => sprite.flipX = flip;
}
