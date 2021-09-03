using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region Variables
    [SerializeField] [Range(5f, 20f)] float impulseForce = 10f;

    // Component
    Rigidbody2D bulletRB;
    Camera _cam;

    // Direction
    float _xAngle;
    float _yAngle;
    Vector2 _aimDirection;
    Vector2 _defaultAimDirection = new Vector2(-1, 0);

    // Bound
    float width;
    float height;
    float _xMaxBound;
    float _xMinBound;
    float _yMaxBound;
    float _yMinBound;
    float _offset;

    // Pause
    bool _hasPaused;
    bool _hasResumed = true;
    Vector2 _previousVelocity;
    #endregion

    #region Starts & Updates
    private void Awake()
    {
        // Initialization
        bulletRB = GetComponent<Rigidbody2D>();

        // Calculate camera's width and heihgt
        _cam = Camera.main;
        _offset = 4f;
        height = _cam.orthographicSize;
        width = height * _cam.aspect;
    }

    private void Update()
    {
        // Calculate the boundaries for the bullet
        _xMaxBound = _cam.transform.position.x + width + _offset;
        _xMinBound = _cam.transform.position.x - width - _offset;
        _yMaxBound = _cam.transform.position.y + height + _offset;
        _yMinBound = _cam.transform.position.y - height - _offset;

        // If out of camera's view, reset the bullet
        if (transform.position.x > _xMaxBound || transform.position.x < _xMinBound ||
                transform.position.y > _yMaxBound || transform.position.y < _yMinBound)
            ResetBullet();
    }

    private void FixedUpdate()
    {
        // Freeze the bullet when game is paused
        if (GameManager.isGamePaused && !_hasPaused)
        {
            _previousVelocity = bulletRB.velocity;
            bulletRB.constraints = RigidbodyConstraints2D.FreezeAll;

            _hasPaused = true;
            _hasResumed = false;
        }
        else if (!GameManager.isGamePaused && !_hasResumed)
        {
            bulletRB.constraints = RigidbodyConstraints2D.FreezeRotation;
            bulletRB.velocity = _previousVelocity;

            _hasResumed = true;
            _hasPaused = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Reset the Bullet if meets an obstacle
        if (other.tag == "Obstacle" || other.tag == "Environnement")
            ResetBullet();
            
        // Destroy the bullet and the obstacle
        else if (other.tag == "Destructible")
        {
            Destructible obstacle = other.GetComponent<Destructible>();
            if (!obstacle.isActive)
                return;

            obstacle.Destroy();
            ResetBullet();
        }
    }
    #endregion

    #region Functions
    public void Shoot(Vector2 dir, Transform spawnPos)
    {
        // Set the position of the bullet
        transform.position = spawnPos.position;
        gameObject.SetActive(true);

        // Calculate the angle of shoot
        _xAngle = dir.x;
        _yAngle = dir.y;

        _aimDirection = new Vector2(_xAngle, _yAngle).normalized;

        // If no angle, use a default one
        if (_xAngle == 0 && _yAngle == 0)
            _aimDirection = _defaultAimDirection;

        bulletRB.AddForce(_aimDirection * -impulseForce, ForceMode2D.Impulse);
    }

    private void ResetBullet()
    {
        gameObject.SetActive(false);
        bulletRB.velocity = Vector2.zero;
    }
    #endregion
}
