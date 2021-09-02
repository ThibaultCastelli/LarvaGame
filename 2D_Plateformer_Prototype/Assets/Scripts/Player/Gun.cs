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

    // Angle and recoil
    Vector2 _gunDirection;
    float _aimAngle;
    float _xRecoil;
    float _yRecoil;
    float _nXOffset;

    Color _defaultColor;

    bool _canFollowTarget = true;
    #endregion

    #region Starts & Updates
    private void Start()
    {
        _defaultColor = sprite.color;
        _nXOffset = -xOffset;
    }

    private void FixedUpdate()
    {
        if (!GameManager.isGamePaused)
        {
            // Gun rotate with the right stick
            _gunDirection = input.GetGunDirection();

            // Calculate aim angle
            if (xOffset < 0 && _gunDirection.x == 0 && _gunDirection.y == 0)
                _aimAngle = -270;
            else
                _aimAngle = (Mathf.Atan2(_gunDirection.y, _gunDirection.x) * Mathf.Rad2Deg) - 90;

            transform.rotation = Quaternion.AngleAxis(_aimAngle, Vector3.forward);

            // Gun following the player
            _targetPos = new Vector2(_target.position.x + xOffset, _target.position.y);
            if (_canFollowTarget)
                transform.position = Vector2.Lerp(transform.position, _targetPos, followSpeed);
        }
    }
    #endregion

    #region Functions
    public void ShootRecoil(Vector2 dir) => StartCoroutine(ShootRecoilCoroutine());
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

    public void FlipSprite(bool flip)
    {
        if (flip)
            xOffset = _nXOffset;
        else
            xOffset = Mathf.Abs(xOffset);
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
