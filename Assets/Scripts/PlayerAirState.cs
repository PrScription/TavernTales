using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        // Allow movement in the air
        float moveX = xInput * player.moveSpeed;
        float moveY = yInput * player.moveSpeed;

        player.SetVelocity(moveX, rb.linearVelocity.y);

        // Transition to idle state when grounded
        if (player.IsGroundDetected())
            stateMachine.ChangeState(player.idleState);
    }
}
