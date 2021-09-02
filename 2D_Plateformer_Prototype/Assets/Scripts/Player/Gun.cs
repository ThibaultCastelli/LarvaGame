using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    #region Variables
    [Header("PROPERTIES")]
    [SerializeField] [Range(0f, 2f)] float recoilAmount = 1f;
    [SerializeField] [Range(0.1f, 1f)] float followSpeed = 0.2f;
    [SerializeField] [Range(-1f, 1f)] float xOffset = 0.5f;
    [SerializeField] [Range(-0.5f, 0.5f)] float yOffset = 0.06f;
    [SerializeField] [Range(1f, 20f)] float speedRecoil = 15f;
    [Space]

    [Header("COMPONENT")]
    [SerializeField] Transform _target;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] PlayerInput input;

    // Positions
    Vector2 _targetPos;
    Vector3 _recoilPos;
    Vector3 _defaultRecoilPos = new Vector3(-1, 0);
    float _defaultXOffset;
    float _previousXOffset;

    // Angle and recoil
    Vector2 _gunDirection;
    Vector2 _previousGunDir;
    float _aimAngle;
    float _xRecoil;
    float _yRecoil;

    Player player;
    Color _defaultColor;

    bool _canFollowTarget = true;
    #endregion

    #region Starts & Updates
    private void Awake()
    {
        player = _target.GetComponent<Player>();
        _defaultXOffset = xOffset;
        _defaultColor = sprite.color;
        _previousXOffset = _defaultXOffset;
    }

    private void Update()
    {
        if (GameManager.isGamePaused)
            return;

        // Gun rotate with the right stick
        _gunDirection = input.GetGunDirection();

        // Calculate aim angle and sprite flipping
        if (!sprite.flipY)
        {
            _aimAngle = _gunDirection == Vector2.zero ? -90 : (Mathf.Atan2(_gunDirection.y, _gunDirection.x) * Mathf.Rad2Deg) - 90;
            if (!sprite.flipX && _gunDirection.x == -1)
                sprite.flipX = true;
            else if (sprite.flipX && _gunDirection.x == 1)
                sprite.flipX = false;
            else if (sprite.flipX && _previousGunDir.x == -1 && _gunDirection.x == 0)
                sprite.flipX = false;
        }
        else
        {
            _aimAngle = _gunDirection == Vector2.zero ? -90 : (Mathf.Atan2(_gunDirection.y, _gunDirection.x) * Mathf.Rad2Deg) + 90;
            if (!sprite.flipX && _gunDirection.x == 1)
                sprite.flipX = true;
            else if (sprite.flipX && _gunDirection.x == -1)
                sprite.flipX = false;
            else if (sprite.flipX && _previousGunDir.x == 1 && _gunDirection.x == 0)
                sprite.flipX = false;
        }

        transform.rotation = Quaternion.AngleAxis(_aimAngle, Vector3.forward);

        // Change the offset of the gun when the player is against a wall to prevent shooting in a wall
        if (player.IsAgainstWallLeft && sprite.flipY)
            xOffset = -_defaultXOffset + 0.2f;
        else if (player.IsAgainstWallRight && !sprite.flipY)
            xOffset = _defaultXOffset - 0.2f;
        else
            xOffset = _previousXOffset;

        // Gun following the player
        _targetPos = new Vector2(_target.position.x + xOffset, _target.position.y + yOffset);
        if (_canFollowTarget)
            transform.position = Vector2.Lerp(transform.position, _targetPos, followSpeed);

        _previousGunDir = _gunDirection;
    }
    #endregion

    #region Functions
    // Event listener OnShoot
    public void ShootRecoil(Vector2 dir) => StartCoroutine(ShootRecoilCoroutine());

    // Event listener OnHideGun
    public void HideGun(bool isHide)
    {
        // Put the gun at the right position and activate
        if (isHide)
        {
            transform.position = new Vector2(_target.position.x + xOffset, _target.position.y);
            sprite.color = _defaultColor;
        }
        else
            sprite.color = new Color(1, 1, 1, 0);
    }

    // Event listener OnFlipSprite
    public void FlipSprite(bool flip)
    {
        if (flip)
        {
            xOffset = -_defaultXOffset;
            _previousXOffset = xOffset;
            sprite.flipY = true;
            return;
        }

        xOffset = _defaultXOffset;
        _previousXOffset = xOffset;
        sprite.flipY = false;
    }
    #endregion

    #region Coroutines
    IEnumerator ShootRecoilCoroutine()
    {
        yield return null;
        _canFollowTarget = false;
        Vector3 startPos = transform.position;

        // Calculate the position of the recoil
        if (_gunDirection.y == 1 || (_gunDirection.y == 1 && (_gunDirection.x == 1 || _gunDirection.x == -1)))
        {
            _xRecoil = -_gunDirection.x * recoilAmount;
            _yRecoil = -_gunDirection.y * recoilAmount;
        }
        else
        {
            _xRecoil = -_gunDirection.x * recoilAmount * 1.5f;
            _yRecoil = -_gunDirection.y * recoilAmount * 1.5f;
        }

        _recoilPos = new Vector3(_xRecoil, _yRecoil);

        // Default position of the recoil
        if (_xRecoil == 0 && _yRecoil == 0)
            _recoilPos = _defaultRecoilPos;

        // Apply the recoil to the current position
        float t = 0f;
        while (t != 1)
        {
            t = Mathf.Clamp(t + Time.deltaTime * speedRecoil, 0, 1);
            transform.position = Vector3.Slerp(transform.position, startPos + _recoilPos, t);
            yield return null;
        }

        _canFollowTarget = true;
    }
    #endregion
}
