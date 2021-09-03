using UnityEngine;
using UnityEngine.InputSystem;

public class PauseState : IState
{
    #region Variables
    Player player;
    Rigidbody2D playerRB;
    PlayerInput playerInput;

    Vector2 _previousVelocity;
    #endregion

    public PauseState(Player player, Rigidbody2D playerRB, PlayerInput playerInput)
    {
        this.player = player;
        this.playerRB = playerRB;
        this.playerInput = playerInput;
    }

    #region Start & Updates
    public void OnEnter()
    {
        playerInput.Controls.Player.Pause.started += Resume;

        _previousVelocity = playerRB.velocity;
        playerRB.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void OnExit()
    {
        playerInput.Controls.Player.Pause.started -= Resume;

        playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;
        playerRB.velocity = _previousVelocity;
    }

    public void Tick() 
    {
        if (!GameManager.isGamePaused)
            player.SetState(player.PreviousState);
    }

    public void FixedTick() { }
    #endregion

    void Resume(InputAction.CallbackContext ctx)
    {
        GameManager.isGamePaused = false;
        player.SetState(player.PreviousState);
    }
}
