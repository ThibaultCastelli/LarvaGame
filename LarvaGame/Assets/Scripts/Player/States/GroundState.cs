using UnityEngine;
using UnityEngine.InputSystem;

public class GroundState : IState
{
    #region Variables
    Player player;
    Rigidbody2D RB;
    PlayerInput input;
    PlayerCollision collision;
    PlayerAudio audio;
    float _timeOnAir = 0;
    #endregion

    public GroundState(Player player, Rigidbody2D playerRB, PlayerInput playerInput, PlayerCollision playerCollision, PlayerAudio playerAudio)
    {
        this.player = player;
        this.RB = playerRB;
        this.input = playerInput;
        this.collision = playerCollision;
        this.audio = playerAudio;
    }

    #region Starts & Updates
    public void OnEnter()
    {
        input.Controls.Player.Shoot.started += Shoot;
        //input.Controls.Player.HideGun.started += HideGun;
        input.Controls.Player.Pause.started += Pause;

        _timeOnAir = 0;
        RB.velocity = Vector2.zero;
    }
    public void OnExit()
    {
        input.Controls.Player.Shoot.started -= Shoot;
        //input.Controls.Player.HideGun.started -= HideGun;
        input.Controls.Player.Pause.started -= Pause;
    }

    public void Tick()
    {
        // Reload ammos
        if (!player.IsReloadingAmmos && player.CurrentAmmos < player.DefaultAmmoCount)
            player.ReloadAmmos();
    }

    public void FixedTick()
    {
        // WALK
        // Player can't jump
        player.Movement.y = RB.velocity.y;
        player.Movement.x = input.GetMovementX() * player.SpeedGround;

        // Apply a smooth acceleration and deceleration
        RB.velocity = Vector2.SmoothDamp(RB.velocity, player.Movement, ref player.PlayerVelocity, player.MovementSmooth);

        // Reset the velocity if it's too low
        if (Mathf.Abs(RB.velocity.x) < 0.1f)
            RB.velocity = new Vector2(0f, RB.velocity.y);

        // Check if on ground
        if (!collision.CheckGroundCollision(player.transform, player.GroundCheckPos))
            _timeOnAir += Time.fixedDeltaTime;
        else
            _timeOnAir = 0;

        if (_timeOnAir > player.TimeToLeaveGround)
            player.SetState(player.AirState);
    }
    #endregion

    #region Functions
    void Shoot(InputAction.CallbackContext ctx)
    {
        if (player.CurrentAmmos > 0)
        {
            audio.OnShootAudio.Play(audio.ShootAudioSource);
            player.ShootRecoil();
        }
        else
            audio.OnEmptyShootAudio.Play(audio.EmptyShootAudioSource);
    }

    void HideGun(InputAction.CallbackContext ctx)
    {
        // Play sound
        player.HideGun();
        player.SetState(player.NoGunState);
    }

    void Pause(InputAction.CallbackContext ctx)
    {
        GameManager.isGamePaused = true;
        player.SetState(player.PauseState);
    }
    #endregion
}
