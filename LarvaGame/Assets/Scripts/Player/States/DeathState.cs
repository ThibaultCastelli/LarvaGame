using UnityEngine;

public class DeathState : IState
{
    Player player;
    Rigidbody2D RB;

    public DeathState (Player player, Rigidbody2D RB)
    {
        this.player = player;
        this.RB = RB;
    }

    public void FixedTick() {}
    public void OnEnter() 
    {
        if (player.PreviousState == player.GroundState)
        {
            // Keep the same direction with speed ground
            float dir = RB.velocity.x > 0 ? 1 : -1;
            RB.velocity = new Vector2(player.SpeedGround * dir, RB.velocity.y);
        }
    }
    public void OnExit() {}
    public void Tick() {}
}
