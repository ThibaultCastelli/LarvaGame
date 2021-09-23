using System.Collections;
using UnityEngine;
using ObserverTC;

enum EnterDirection
{
    GO_TO_LEFT,
    GO_TO_RIGHT,
    FALL
}

public class Player : StateMachine
{
    #region Serialize Variables
    [Header("MOVEMENT")]
    [SerializeField] [Range(0.1f, 5f)] float speedGround = 1.5f;
    [SerializeField] [Range(0f, 0.3f)] float movementSmooth = 0.05f;
    [SerializeField] [Range(0f, 0.3f)] float movementSmoothAir = 0.05f;
    [SerializeField] [Range(0.5f, 5f)] float airAcceleration = 1f;
    [Space]
    [SerializeField] [Range(2f, 20f)] float maxAirSpeedHorizontal = 7f;
    [SerializeField] [Range(2f, 20f)] float maxAirSpeedVertical = 7f;
    [SerializeField] [Range(2f, 20f)] float maxAirSpeedDiagonal = 10f;
    [SerializeField] [Range(2f, 20f)] float maxFallSpeed = 10f;
    [Space]
    [SerializeField] [Range(1f, 10f)] float speedClimb = 6f;
    [SerializeField] [Range(0.01f, 0.5f)] float timeToLeaveGround = 0.1f;
    [Space]

    [Header("ENTER STATE")]
    [SerializeField] EnterDirection enterDirection = EnterDirection.GO_TO_RIGHT;
    [SerializeField] [Range(0f, 5f)] float timeOfEnterState = 1f;
    [Space]

    [Header("COMPONENTS")]
    [SerializeField] PlayerVisual playerVisual;
    [SerializeField] PlayerAudio playerAudio;
    [Space]

    [Header("COLLIDERS")]
    [SerializeField] Transform groundCheckPos;
    [SerializeField] Transform plateformCheckLeftPos;
    [SerializeField] Transform plateformCheckRightPos;
    [SerializeField] [Range(0, 1)] float timeToTriggerDeath = 1;
    [Space]

    [Header("SHOOT")]
    [SerializeField] [Range(5f, 30f)] float verticalShootPower = 10f;
    [SerializeField] [Range(5f, 30f)] float horizontalShootPower = 10f;
    [SerializeField] [Range(5f, 30f)] float diagonalShootPower = 10f;
    [SerializeField] [Range(0f, 1f)] float timeOfFreeze = 0.1f;
    [SerializeField] [Range(0f, 2f)] float timeToReloadAmmos = 0.2f;
    [Space]

    [Header("EVENTS")]
    [SerializeField] NotifierVector2 shootEvent;
    [SerializeField] NotifierGameObject ammoChangeEvent;
    //[SerializeField] EventBool hideGunEvent;
    [SerializeField] Notifier deathEvent;
    #endregion

    #region Variables
    // Components
    Rigidbody2D playerRB;
    PlayerCollision playerCollision;
    PlayerInput playerInput;

    // States
    public IState GroundState { get; private set; }
    public IState AirState { get; private set; }
    public IState NoGunState { get; private set; }
    public IState ClimbState { get; private set; }
    public IState PauseState { get; private set; }
    public IState DeathState { get; private set; }

    // Movement
    [HideInInspector] public Vector2 Movement;
    [HideInInspector] public Vector2 PlayerVelocity = Vector2.zero;
    public bool CanMove { get; private set; } = true;

    // Aim
    Vector2 _defaultAimDirection = new Vector2(-1, 0);
    Vector2 _aimDirection;

    // Collisions
    public Collider2D PlateformColliderLeft { get; private set; }
    public Collider2D PlateformColliderRight { get; private set; }
    public bool IsAgainstWallLeft { get; private set; }
    public bool IsAgainstWallRight { get; private set; }

    // Ammos
    public bool IsReloadingAmmos { get; private set; }
    public int CurrentAmmos { get; private set; }
    public int DefaultAmmoCount { get; private set; } = 2;

    // Shoot
    public float PreviousShootPower { get; private set; }
    //bool _gunInHand = true;
    #endregion

    #region Getters
    public Transform GroundCheckPos { get { return groundCheckPos; } }
    public float SpeedGround { get { return speedGround; } }
    public float MovementSmooth { get { return movementSmooth; } }
    public float MovementSmoothAir { get { return movementSmoothAir; } }
    public float AirAcceleration { get { return airAcceleration; } }
    public float VerticalShootPower { get { return verticalShootPower; } }
    public float MaxAirSpeedVertical { get { return maxAirSpeedVertical; } }
    public float HorizontalShootPower { get { return horizontalShootPower; } }
    public float MaxAirSpeedHorizontal { get { return maxAirSpeedHorizontal; } }
    public float DiagonalShootPower { get { return diagonalShootPower; } }
    public float MaxAirSpeedDiagonal { get { return maxAirSpeedDiagonal; } }
    public float MaxFallSpeed { get { return maxFallSpeed; } }
    public float TimeToLeaveGround { get { return timeToLeaveGround; } }
    #endregion

    #region Starts & Updates
    private void Awake()
    {
        // Instantiate components
        playerRB = GetComponent<Rigidbody2D>();
        playerCollision = GetComponent<PlayerCollision>();
        playerInput = GetComponent<PlayerInput>();

        // Instantiate states
        GroundState = new GroundState(this, playerRB, playerInput, playerCollision, playerAudio);
        AirState = new OnAirState(this, playerRB, playerInput, playerCollision, playerAudio);
        NoGunState = new NoGunState(this, playerRB, playerInput);
        ClimbState = new ClimbingState(this, playerRB, playerInput);
        PauseState = new PauseState(this, playerRB, playerInput);
        DeathState = new DeathState(this, playerRB);

        // Initialize variables
        CurrentAmmos = DefaultAmmoCount;
    }

    private void Start()
    {
        StartCoroutine(EnterState());
    }

    protected override void Update()
    {
        base.Update();

        if (playerVisual.FlipX && _defaultAimDirection.x < 0)
            _defaultAimDirection = new Vector2(1, 0);
        else if (!playerVisual.FlipX && _defaultAimDirection.x > 0)
            _defaultAimDirection = new Vector2(-1, 0);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        IsAgainstWallLeft = playerCollision.CheckWallCollision(true, this.transform);
        IsAgainstWallRight = playerCollision.CheckWallCollision(false, this.transform);

        PlateformColliderLeft = playerCollision.CheckClimbCollision(plateformCheckLeftPos);
        PlateformColliderRight = playerCollision.CheckClimbCollision(plateformCheckRightPos);
    }
    #endregion

    #region Functions
    public void ShootRecoil()
    {
        // Initialize aim direction and reset velocity
        _aimDirection = playerInput.GetAim();
        if (_aimDirection == Vector2.zero)
            _aimDirection = _defaultAimDirection;
        else if (_aimDirection != Vector2.up && _aimDirection != Vector2.down)
            playerRB.velocity = Vector2.zero;

        Vector2 bulletDirection = _aimDirection;

        // Vertical direction
        if (_aimDirection == Vector2.up || _aimDirection == Vector2.down)
            PreviousShootPower = verticalShootPower;

        // Horizontal direction
        else if (_aimDirection == Vector2.right || _aimDirection == Vector2.left || _aimDirection == _defaultAimDirection)
        {
            PreviousShootPower = horizontalShootPower;

            // If against a wall, prevent from boucing
            if ((IsAgainstWallLeft && _aimDirection == Vector2.left) || (IsAgainstWallRight && _aimDirection == Vector2.right))
                _aimDirection = new Vector2(0, 0.4f);
            // Give a small jump to the horizontal shoot
            else
                _aimDirection = new Vector2(_aimDirection.x, 0.4f);
        }

        // Diagonal direction
        else
        {
            PreviousShootPower = diagonalShootPower;

            // If against a wall, prevent from bouncing
            if ((IsAgainstWallLeft && _aimDirection.x < 0) || (IsAgainstWallRight && _aimDirection.x > 0))
                _aimDirection = new Vector2(0, _aimDirection.y * 1.15f);
            // Ajust the trajectory of the diagonal shoot
            else
                _aimDirection = new Vector2(_aimDirection.x * 0.7f, _aimDirection.y * 1.15f);
        }

        // Recoil
        SetState(AirState);
        playerRB.AddForce(_aimDirection * PreviousShootPower, ForceMode2D.Impulse);

        // Update ammo count and raise shoot event
        CurrentAmmos--;
        shootEvent.Notify(bulletDirection);
        ammoChangeEvent.Notify(this.gameObject);
    }

    public void ShootFreeze() => StartCoroutine(ShootFreezeCoroutine());
    public void ReloadAmmos() => StartCoroutine(ReloadAmmosCoroutine());
    //public void HideGun() => hideGunEvent.Raise(_gunInHand = !_gunInHand);
    public void ClimbPlateform(Collider2D plateformCollider, bool goToLeft) => StartCoroutine(ClimbPlateformCoroutine(plateformCollider, goToLeft));
    #endregion

    #region Event Functions
    public void CollectAmmo(GameObject ammo)
    {
        CurrentAmmos++;
        ammoChangeEvent.Notify(this.gameObject);
    }

    public void Death()
    {
        StopAllCoroutines();
        SetState(DeathState);
        gameObject.SetActive(false);
    }
    #endregion

    #region Coroutines
    IEnumerator EnterState()
    {
        SetState(DeathState);

        switch (enterDirection)
        {
            case EnterDirection.GO_TO_LEFT:
                playerRB.velocity = new Vector2(-speedGround, 0);
                break;
            case EnterDirection.GO_TO_RIGHT:
                playerRB.velocity = new Vector2(speedGround, 0);
                break;
            case EnterDirection.FALL:
                break;
        }

        yield return new WaitForSeconds(timeOfEnterState);

        SetState(GroundState);
    }

    IEnumerator ShootFreezeCoroutine()
    {
        float previousVelocityX = playerRB.velocity.x;

        // Freeze the player
        CanMove = false;
        playerRB.velocity = Vector2.zero;
        playerRB.gravityScale = 0;

        yield return new WaitForSeconds(timeOfFreeze);
        while (GameManager.isGamePaused)
            yield return new WaitForSeconds(timeOfFreeze);

        // Reset movement and shoot
        CanMove = true;
        playerRB.gravityScale = 3;

        // Keep velocity if shoot up or down and go to the same previous direction
        if (playerInput.GetMovementX() * previousVelocityX >= 0 && (playerInput.GetAim() == Vector2.up || playerInput.GetAim() == Vector2.down))
            playerRB.velocity = new Vector2(previousVelocityX, playerRB.velocity.y);

        ShootRecoil();
    }

    IEnumerator ReloadAmmosCoroutine()
    {
        // Prevent to reload more than 1 time
        IsReloadingAmmos = true;

        yield return new WaitForSeconds(timeToReloadAmmos);

        CurrentAmmos = DefaultAmmoCount;
        ammoChangeEvent.Notify(this.gameObject);

        IsReloadingAmmos = false;
    }

    IEnumerator ClimbPlateformCoroutine(Collider2D plateformCollider, bool goToLeft)
    {
        // Initialization
        float t = 0f;
        Vector3 startPos = transform.position;
        Vector3 posToClimb;

        // Freeze player
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
            t = Mathf.Clamp(t + Time.fixedDeltaTime * speedClimb, 0, 1);
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
            t = Mathf.Clamp(t + Time.fixedDeltaTime * (speedClimb / 2), 0, 1);
            playerRB.position = Vector2.Lerp(startPos, posToClimb, t);
            yield return null;
        }

        // Reset the movement for the player
        playerRB.gravityScale = 3f;
        SetState(GroundState);
    }

    public IEnumerator DeathCoroutine()
    {
        yield return new WaitForSeconds(timeToTriggerDeath);
        while (GameManager.isGamePaused)
            yield return null;
        deathEvent.Notify();
    }
    #endregion
}
