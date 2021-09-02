using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{/*
    #region Variables
    [Header("MOVEMENT")]
    [SerializeField] [Range(1f, 15f)] float speedGround = 5f;
    [SerializeField] [Range(0.5f, 5f)] float accelerationAir = 1f;
    [SerializeField] [Range(0f, 0.3f)] float movementSmooth = 0.05f;
    [SerializeField] [Range(2f, 20f)] float maxAirSpeedHorizontal = 7f;
    [SerializeField] [Range(2f, 20f)] float maxAirSpeedVertical = 7f;
    [SerializeField] [Range(2f, 20f)] float maxAirSpeedDiagonal = 10f;
    [Space]

    [Header("SHOOT")]
    [SerializeField] [Range(5f, 30f)] float verticalShootPower = 10f;
    [SerializeField] [Range(5f, 30f)] float horizontalShootPower = 10f;
    [SerializeField] [Range(5f, 30f)] float diagonalShootPower = 10f;
    [SerializeField] [Range(0f, 1f)] float timeOfFreeze = 0.2f;
    [SerializeField] [Range(0f, 2f)] float timeToReloadAmmos = 0.2f;
    [SerializeField] [Range(0, 5)] int defaultAmmoCount = 2;
    [Space]

    [Header("COLLIDERS")]
    [SerializeField] Transform groundCheckPos;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] [Range(0.01f, 0.5f)] float timeToLeaveGround = 0.1f;
    [SerializeField] Transform plateformCheckLeftPos;
    [SerializeField] Transform plateformCheckRightPos;
    [SerializeField] LayerMask plateformCheckLayer;
    [SerializeField] [Range(0.1f, 4f)] float timeToClimb = 1.5f;
    [Space]

    [Header("COMPONENT")]
    [SerializeField] Rigidbody2D playerRB;
    [SerializeField] SpriteRenderer sprite;
    [Space]

    [Header("AUDIO")]
    [SerializeField] SimpleAudioEvent onShootAudio;
    [SerializeField] SimpleAudioEvent onShootAirAudio;
    [SerializeField] AudioSource shootAudioSource;
    [Space]

    [Header("EVENTS")]
    [SerializeField] GameEvent shootEvent;
    [SerializeField] GameEvent playerDeathEvent;
    [SerializeField] EventPlayer ammoChangeEvent;
    [SerializeField] EventPlayer hideGunEvent;
    [SerializeField] GameEventPlayerGameObject collectAmmoEvent;

    // Movement
    Vector2 _movement;
    Vector2 _playerVelocity = Vector2.zero;
    Vector2 _previousVelocity;
    bool _canMove = true;

    // Aim
    Vector2 _defaultAimDirection = new Vector2(-1, 0);
    Vector2 _aimDirection;
    
    // Shoot
    bool _canShoot = true;
    bool _canShootWithoutFreeze = true;
    bool _isReloadingAmmo = false;
    bool _gunInHand = true;
    int _currentAmmos;
    float _previousShootPower = 0f;

    // Collision
    bool _isOnGround;
    bool _isAgainstWallLeft;
    bool _isAgainstWallRight;
    bool _isClimbing;
    float _leaveGroundCount = 0f;
    Collider2D _previousGround = null;
    GameObject _leftPlateformCollider;
    GameObject _rightPlateformCollider;

    // Pause
    bool _hasResume = true;
    bool _hasPaused = false;

    // Properties
    public Vector2 AimDirection { get { return _aimDirection; } }
    public bool IsOnGround { get { return _isOnGround; } }
    public bool IsGunInHand { get { return _gunInHand; } }
    public int CurrentAmmos { get { return _currentAmmos; } }

    // Events
    Action<InputAction.CallbackContext> DefaultAim;
    #endregion

    #region Starts & Updates
    private void Awake()
    {
        // Initialize variables
        _currentAmmos = defaultAmmoCount;
        _aimDirection = _defaultAimDirection;
    }

    private void Start()
    {
        // Lambda expressions
        DefaultAim = ctx => _aimDirection = _defaultAimDirection;

        // Subscribe to events
        EventManager.instance.controls.Player.Aim.performed += Aim;
        EventManager.instance.controls.Player.Aim.canceled += DefaultAim;

        EventManager.instance.controls.Player.Shoot.started += Shoot;

        EventManager.instance.controls.Player.HideGun.started += HideGun;
    }

    private void OnDisable()
    {
        // Unsubscribe to events
        EventManager.instance.controls.Player.Aim.performed -= Aim;
        EventManager.instance.controls.Player.Aim.canceled -= DefaultAim;

        EventManager.instance.controls.Player.Shoot.started -= Shoot;

        EventManager.instance.controls.Player.HideGun.started -= HideGun;
    }

    private void Update()
    {
        // Check if the player has ammos
        if (_currentAmmos <= 0)
            _canShoot = false;
        else
            _canShoot = true;

        // Reload ammos and slow motion
        if (_isOnGround && !_isReloadingAmmo && _currentAmmos < defaultAmmoCount)
            StartCoroutine(ReloadAmmos());

        // Flip sprite
        if (!GameManager.isGamePaused)
        {
            // Flip when moving to the left
            if (EventManager.instance.controls.Player.Move.ReadValue<Vector2>().x < 0)
                sprite.flipX = true;
            else if (EventManager.instance.controls.Player.Move.ReadValue<Vector2>().x > 0)
                sprite.flipX = false;

            // Flip when aiming to the left
            if (EventManager.instance.controls.Player.Aim.ReadValue<Vector2>().x < -0.5)
                sprite.flipX = true;
            else if (EventManager.instance.controls.Player.Aim.ReadValue<Vector2>().x > 0.5)
                sprite.flipX = false;
        }
    }
    
    private void FixedUpdate()
    {
        // COLLISIONS
        // Check if on ground
        Collider2D groundCollider = Physics2D.OverlapBox(groundCheckPos.position, new Vector2(transform.localScale.x * 0.95f, transform.localScale.y / 10), 0, groundLayer);
        
        // If not on ground
        if (groundCollider == null)
        {
            _isOnGround = false;

            // Add a delay to continue to shoot without freeze
            _leaveGroundCount = Mathf.Clamp(_leaveGroundCount + Time.deltaTime, 0, timeToLeaveGround);

            // If delay is past, the next shoot will freeze the player
            // Also re-activate the teleport collider of the plateform where the player was.
            if (_leaveGroundCount == timeToLeaveGround)
            {
                _canShootWithoutFreeze = false;

                if (_leftPlateformCollider != null)
                    _leftPlateformCollider.gameObject.SetActive(true);
                if (_rightPlateformCollider != null)
                    _rightPlateformCollider.gameObject.SetActive(true);
            }
        }
        // If on ground
        else
        {
            _isOnGround = true;
            _canShootWithoutFreeze = true;

            if (groundCollider != _previousGround)
            {
                // Reset the previous plateform collider
                if (_previousGround != null && _previousGround.TryGetComponent(out Climbable previousPlateform))
                    previousPlateform.ResetColliders();

                // Find if the plateform the player is on has telport collider
                if (groundCollider.TryGetComponent(out Climbable currentPlateformClimbable))
                {
                    _leftPlateformCollider = currentPlateformClimbable.colliderLeft;
                    _rightPlateformCollider = currentPlateformClimbable.colliderRight;
                }
                else
                {
                    _leftPlateformCollider = null;
                    _rightPlateformCollider = null;
                }
            }

            // If so, desactivate them
            if (_leftPlateformCollider != null)
                _leftPlateformCollider.gameObject.SetActive(false);
            if (_rightPlateformCollider != null)
                _rightPlateformCollider.gameObject.SetActive(false);

            _previousGround = groundCollider;
            // Reset the delay for the shoot freeze
            _leaveGroundCount = 0f;
        }

        // Check if the player collide with the top side of a plateform
        Collider2D plateformColliderRight = Physics2D.OverlapBox(plateformCheckRightPos.position,
            new Vector2(0.05f, 0.1f), 0, plateformCheckLayer);
        Collider2D plateformColliderLeft = Physics2D.OverlapBox(plateformCheckLeftPos.position,
            new Vector2(0.05f, 0.1f), 0, plateformCheckLayer);

        // If so, teleport the player to the top of the plateform
        if (!_isClimbing && playerRB.velocity.y < 2)
        {
            if (plateformColliderRight != null && EventManager.instance.controls.Player.Move.ReadValue<Vector2>().x > 0)
                StartCoroutine(ClimbPlateform(plateformColliderRight));
            else if (plateformColliderLeft != null && EventManager.instance.controls.Player.Move.ReadValue<Vector2>().x < 0)
                StartCoroutine(ClimbPlateform(plateformColliderLeft, true));
        }

        // Check if the player is against a wall to prevent bouncing
        RaycastHit2D hit;
        
        hit = Physics2D.Raycast(transform.position, Vector2.left, transform.localScale.x * 0.7f, groundLayer);
        
        if (hit.collider != null)
        {
            _isAgainstWallLeft = true;
            _isAgainstWallRight = false;
        }
        else
        {
            // If no wall to the left, check to the right
            hit = Physics2D.Raycast(transform.position, Vector2.right, transform.localScale.x * 0.7f, groundLayer);
            if (hit.collider != null)
            {
                _isAgainstWallRight = true;
                _isAgainstWallLeft = false;
            }
                
            else
                _isAgainstWallRight = false;
                _isAgainstWallLeft = false;
        }

        // PLAYER MOVEMENT
        // Player can't jump
        _movement.y = playerRB.velocity.y;

        // Change the speed if on ground or in the air
        if (_canMove && !GameManager.isGamePaused)
        {
            if (_isOnGround)
                _movement.x = Mathf.Round(EventManager.instance.controls.Player.Move.ReadValue<Vector2>().x) * speedGround;
            else
            {
                _movement.x = Mathf.Round(EventManager.instance.controls.Player.Move.ReadValue<Vector2>().x) * accelerationAir + playerRB.velocity.x;

                // Clamp the horizontal speed on air
                if (_previousShootPower == verticalShootPower)
                    _movement.x = Mathf.Clamp(_movement.x, -maxAirSpeedVertical, maxAirSpeedVertical);
                else if (_previousShootPower == diagonalShootPower)
                    _movement.x = Mathf.Clamp(_movement.x, -maxAirSpeedDiagonal, maxAirSpeedDiagonal);
                else
                    _movement.x = Mathf.Clamp(_movement.x, -maxAirSpeedHorizontal, maxAirSpeedHorizontal);
            }

            // Apply a smooth acceleration and deceleration
            playerRB.velocity = Vector2.SmoothDamp(playerRB.velocity, _movement, ref _playerVelocity, movementSmooth);

            // Reset the velocity it's too low
            if (Mathf.Abs(playerRB.velocity.x) < 0.1f)
                playerRB.velocity = new Vector2(0f, playerRB.velocity.y);
        }
        // Stop the player if can't move
        else
            playerRB.velocity = Vector2.SmoothDamp(playerRB.velocity, new Vector2(0, _movement.y), ref _playerVelocity, movementSmooth);

        // GAME PAUSE
        if (GameManager.isGamePaused && !_hasPaused)
        {
            _hasPaused = true;
            _hasResume = false;

            // If the game is paused, get the velocity and freeze the rigidbody
            _previousVelocity = playerRB.velocity;
            playerRB.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else if (!GameManager.isGamePaused && !_hasResume)
        {
            _hasResume = true;
            _hasPaused = false;

            // If the game is resumed, reset the constraints and reset the velocity saved before the pause
            playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;
            playerRB.velocity = _previousVelocity;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Ammo":
                // Check if the parent have the Ammo script
                Ammo parent = collision.gameObject.transform.parent.GetComponent<Ammo>();
                if (parent != null)
                {
                    // Destroy the ammo and add one to the player
                    _currentAmmos++;
                    collectAmmoEvent.Raise(this, collision.gameObject);
                    ammoChangeEvent.Raise(this);
                }
                break;

            case "Kill":
                Death();
                break;
        }
    }
    #endregion

    #region Functions
    void Aim(InputAction.CallbackContext ctx)
    {
        if (!GameManager.isGamePaused)
        {
            // Get only 8 directions
            float xAngle = -Mathf.Round(ctx.ReadValue<Vector2>().x);
            float yAngle = -Mathf.Round(ctx.ReadValue<Vector2>().y);

            _aimDirection = new Vector2(xAngle, yAngle);
        }
    }

    void Shoot(InputAction.CallbackContext ctx)
    {
        // Freeze the player if in the air
        if (_canShoot && _gunInHand && !GameManager.isGamePaused)
        {
            if (_canShootWithoutFreeze && _currentAmmos == 2)
            {
                onShootAudio.Play(shootAudioSource);
                ShootRecoil();
            }
                
                
            else
            {
                onShootAirAudio.Play(shootAudioSource);
                StartCoroutine(ShootFreeze());
            }
        }
    }

    void ShootRecoil()
    {
        Vector2 _previousAimDirection = _defaultAimDirection;

        // Use different shoot power for horizontal, vertical, or diagonal direction
        // Vertical direction
        if (_aimDirection == Vector2.up || _aimDirection == Vector2.down)
        {
            _previousShootPower = verticalShootPower;
            _previousAimDirection = _aimDirection;
        }

        // Horizontal direction
        else if (_aimDirection == Vector2.right || _aimDirection == Vector2.left || _aimDirection == _defaultAimDirection)
        {
            _previousShootPower = horizontalShootPower;
            _previousAimDirection = _aimDirection;

            // If against a wall, prevent from boucing
            if ((_isAgainstWallLeft && _aimDirection == Vector2.left) || (_isAgainstWallRight && _aimDirection == Vector2.right))
                _aimDirection = new Vector2(0, 0.4f);
            // Give a small jump to the horizontal shoot
            else
                _aimDirection = new Vector2(_aimDirection.x, 0.4f);
        }

        // Diagonal direction
        else
        {
            _previousAimDirection = _aimDirection;
            _previousShootPower = diagonalShootPower;

            // If against a wall, prevent from bouncing
            if ((_isAgainstWallLeft && _aimDirection.x < 0) || (_isAgainstWallRight && _aimDirection.x > 0))
                _aimDirection = new Vector2(0, _aimDirection.y * 1.1f);
            // Ajust the trajectory of the diagonal shoot
            else
                _aimDirection = new Vector2(_aimDirection.x * 0.7f, _aimDirection.y * 1.1f);
        }
            
        // Recoil
        playerRB.AddForce(_aimDirection * _previousShootPower, ForceMode2D.Impulse);

        // Update ammo count and raise shoot event
        _currentAmmos--;
        shootEvent.Raise();
        ammoChangeEvent.Raise(this);

        // Reset the aim direction
        _aimDirection = _previousAimDirection;
    }

    void HideGun(InputAction.CallbackContext ctx)
    {
        if (!GameManager.isGamePaused)
        {
            _gunInHand = !_gunInHand;
            hideGunEvent.Raise(this);
        }
    }

    public void Death()
    {
        playerDeathEvent.Raise();
        gameObject.SetActive(false);
    }
    #endregion

    #region Coroutines
    IEnumerator ReloadAmmos()
    {
        // Prevent to reload more than 1 time
        _isReloadingAmmo = true;

        yield return new WaitForSeconds(timeToReloadAmmos);

        // Reload only on ground
        if (_isOnGround)
        {
            _currentAmmos = defaultAmmoCount;
            ammoChangeEvent.Raise(this);
        }

        _isReloadingAmmo = false;
    }

    IEnumerator ShootFreeze()
    {
        // Prevent the player to move during the freeze
        _canMove = false;

        // Conserve the horizontal velocity only if doing a vertical shoot and move to the opposite direction
        float xVelocity = 0f; ;
        if (playerRB.velocity.x * EventManager.instance.controls.Player.Move.ReadValue<Vector2>().x > 0 && _aimDirection == Vector2.up || _aimDirection == Vector2.down)
            xVelocity = playerRB.velocity.x;
            
        playerRB.velocity = Vector2.zero;
        playerRB.gravityScale = 0;

        yield return new WaitForSeconds(timeOfFreeze);
        while (GameManager.isGamePaused)
            yield return new WaitForSeconds(timeOfFreeze);

        // Reset movement and shoot
        _canMove = true;
        playerRB.gravityScale = 3;
        playerRB.velocity = new Vector2(xVelocity, 0);

        ShootRecoil();
    }

    IEnumerator ClimbPlateform(Collider2D plateformCollider, bool goToLeft = false)
    {
        // Initialization
        float t = 0f;
        Vector3 startPos = transform.position;
        Vector3 posToClimb;

        // Avoid to run this coroutin multiple time
        _isClimbing = true;

        // Stop the player movement and prevent from moving and shooting
        _canMove = false;
        _canShoot = false;
        playerRB.velocity = Vector2.zero;
        playerRB.gravityScale = 0f;

        // Calculate the position to climb on the Y axis
        float topOfPlateform = (plateformCollider.transform.position.y - plateformCheckRightPos.position.y) +
               (plateformCollider.GetComponent<SpriteRenderer>().bounds.size.y / 2) +
               (plateformCheckRightPos.GetComponent<SpriteRenderer>().bounds.size.y / 2);

        posToClimb = new Vector3(transform.position.x, transform.position.y + topOfPlateform);

        // Climb on the Y axis
        while (t != 1)
        {
            t = Mathf.Clamp(t + Time.fixedDeltaTime * timeToClimb, 0, 1);
            playerRB.position = Vector2.Lerp(startPos, posToClimb, t);
            yield return null;
        }
        
        // Calculate the position to climb on the X axis
        if (!goToLeft)
            posToClimb = new Vector3(transform.position.x + 0.15f, transform.position.y);
        else
            posToClimb = new Vector3(transform.position.x - 0.15f, transform.position.y);

        // Reset the variables
        t = 0f;
        startPos = transform.position;

        // Climb on the X axis
        while (t != 1)
        {
            t = Mathf.Clamp(t + Time.fixedDeltaTime * timeToClimb, 0, 1);
            playerRB.position = Vector2.Lerp(startPos, posToClimb, t);
            yield return null;
        }

        // Reset the movement for the player
        _canMove = true;
        _canShoot = true;
        _isOnGround = true;
        _isClimbing = false;
        playerRB.gravityScale = 3f;
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
    }*/
}
