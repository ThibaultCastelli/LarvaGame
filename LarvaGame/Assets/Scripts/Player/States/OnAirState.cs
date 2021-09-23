using UnityEngine;
using UnityEngine.InputSystem;

public class OnAirState : IState
{
    #region Variables
    Player player;
    Rigidbody2D RB;
    PlayerInput input;
    PlayerCollision collision;
    PlayerAudio audio;

    bool _isOnGround = false;
    float _frameInAir = 0;
    #endregion

    public OnAirState(Player player, Rigidbody2D playerRB, PlayerInput playerInput, PlayerCollision playerCollision, PlayerAudio audio)
    {
        this.player = player;
        this.RB = playerRB;
        this.input = playerInput;
        this.collision = playerCollision;
        this.audio = audio;
    }

    #region Starts & Updates
    public void OnEnter()
    {
        input.Controls.Player.Shoot.started += Shoot;
        input.Controls.Player.Pause.started += Pause;

        _frameInAir = 0;
        _isOnGround = false;
    }

    public void OnExit()
    {
        input.Controls.Player.Shoot.started -= Shoot;
        input.Controls.Player.Pause.started -= Pause;
    }

    public void FixedTick()
    {
        if (_isOnGround)
            player.SetState(player.GroundState);

        // Air Movement
        player.Movement.y = RB.velocity.y;

        if (player.CanMove)
        {
            player.Movement.x = input.GetMovementX() * player.AirAcceleration + RB.velocity.x;

            // Clamp the horizontal speed on air
            if (player.PreviousShootPower == player.VerticalShootPower)
                player.Movement.x = Mathf.Clamp(player.Movement.x, -player.MaxAirSpeedVertical, player.MaxAirSpeedVertical);
            else if (player.PreviousShootPower == player.DiagonalShootPower)
                player.Movement.x = Mathf.Clamp(player.Movement.x, -player.MaxAirSpeedDiagonal, player.MaxAirSpeedDiagonal);
            else
                player.Movement.x = Mathf.Clamp(player.Movement.x, -player.MaxAirSpeedHorizontal, player.MaxAirSpeedHorizontal);

            // Clamp fall speed
            player.Movement.y = Mathf.Clamp(player.Movement.y, -player.MaxFallSpeed, player.MaxFallSpeed);

            // Apply a smooth acceleration and deceleration
            RB.velocity = Vector2.SmoothDamp(RB.velocity, player.Movement, ref player.PlayerVelocity, player.MovementSmoothAir);

            // Reset the velocity if it's too low
            if (Mathf.Abs(RB.velocity.x) < 0.1f)
                RB.velocity = new Vector2(0f, RB.velocity.y);
        }
        // Stop the player if can't move
        else
            RB.velocity = Vector2.zero;

        // Leave 1 frame before checking if on ground
        if (collision.CheckGroundCollision(player.transform, player.GroundCheckPos) != null && _frameInAir > 10)
            _isOnGround = true;

        // Change to climbing state
        if (RB.velocity.y < 2 && ((player.PlateformColliderRight != null && input.GetMovementX() > 0) || (player.PlateformColliderLeft != null && input.GetMovementX() < 0)))
            player.SetState(player.ClimbState);
    }

    public void Tick()
    {
        _frameInAir++;
    }
    #endregion

    #region Functions
    void Shoot(InputAction.CallbackContext ctx)
    {
        if (player.CurrentAmmos > 0)
        {
            audio.OnShootAirSFX.Play();
            player.ShootFreeze();
        }
        else
            audio.OnEmptyShootSFX.Play();
    }

    void Pause(InputAction.CallbackContext ctx)
    {
        GameManager.isGamePaused = true;
        player.SetState(player.PauseState);
    }
    #endregion
}
