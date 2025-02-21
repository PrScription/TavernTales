using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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

        // Handle 4-directional movement
        float moveX = xInput * player.moveSpeed;
        float moveY = yInput * player.moveSpeed;

        // Apply movement
        player.SetVelocity(moveX, moveY);

        // Transition to idle state if no input
        if (xInput == 0 && yInput == 0)
            stateMachine.ChangeState(player.idleState);
    }
}

