using UnityEngine;
using UnityEngine.InputSystem;

public class ClimbingState : IState
{
    #region Variables
    Player player;
    Rigidbody2D RB;
    PlayerInput input;
    #endregion

    public ClimbingState(Player player, Rigidbody2D playerRB, PlayerInput playerInput)
    {
        this.player = player;
        this.RB = playerRB;
        this.input = playerInput;
    }

    #region Starts & Updates
    public void OnEnter()
    {
        input.Controls.Player.Pause.started += Pause;

        if (player.PlateformColliderRight != null && input.GetMovementX() > 0)
            player.ClimbPlateform(player.PlateformColliderRight, false);
        else if (player.PlateformColliderLeft != null && input.GetMovementX() < 0)
            player.ClimbPlateform(player.PlateformColliderLeft, true);
    }

    public void OnExit()
    {
        input.Controls.Player.Pause.started -= Pause;
    }

    public void FixedTick() {}
    public void Tick() {}
    #endregion

    void Pause(InputAction.CallbackContext ctx)
    {
        GameManager.isGamePaused = true;
        player.SetState(player.PauseState);
    }
}
