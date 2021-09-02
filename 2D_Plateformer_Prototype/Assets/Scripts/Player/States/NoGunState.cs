using UnityEngine;
using UnityEngine.InputSystem;

public class NoGunState : IState
{
    #region Variables
    Player player;
    Rigidbody2D playerRB;
    PlayerInput playerInput;
    #endregion

    public NoGunState(Player player, Rigidbody2D playerRB, PlayerInput playerInput)
    {
        this.player = player;
        this.playerRB = playerRB;
        this.playerInput = playerInput;
    }

    #region Starts & Updates
    public void OnEnter()
    {
        playerInput.Controls.Player.HideGun.started += TakeGun;
        playerInput.Controls.Player.Pause.started += Pause;

    }

    public void OnExit()
    {
        playerInput.Controls.Player.HideGun.started -= TakeGun;
        playerInput.Controls.Player.Pause.started -= Pause;
    }

    public void FixedTick()
    {
        // Player movement
        player.Movement.y = playerRB.velocity.y;
        player.Movement.x = playerInput.GetMovementX() * player.SpeedGround;

        // Apply a smooth acceleration and deceleration
        playerRB.velocity = Vector2.SmoothDamp(playerRB.velocity, player.Movement, ref player.PlayerVelocity, player.MovementSmooth);

        // Reset the velocity if it's too low
        if (Mathf.Abs(playerRB.velocity.x) < 0.1f)
            playerRB.velocity = new Vector2(0f, playerRB.velocity.y);
    }

    public void Tick() {}
    #endregion

    #region Functions
    void TakeGun(InputAction.CallbackContext ctx)
    {
        // Play sound
        player.HideGun();
        player.SetState(player.GroundState);
    }

    void Pause(InputAction.CallbackContext ctx)
    {
        GameManager.isGamePaused = true;
        player.SetState(player.PauseState);
    }
    #endregion
}
