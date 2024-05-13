using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        InputManager.instance.inputControl.GamePlay.Jump.started += wallJump;
    }

    public override void Exit()
    {
        base.Exit();

        InputManager.instance.inputControl.GamePlay.Jump.started -= wallJump;
    }

    public override void Update()
    {
        base.Update();

        if (!player.IsWallDetected())
        {
            stateMachine.ChangeState(player.airState);
        }

        if (xInput != 0 && player.facingDir*xInput < 0)//判断此时移动的发现是否和facingDir不一致
        {
            Debug.Log(1);
            stateMachine.ChangeState(player.idleState);
        }

        if (yInput < 0)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y * 0.7f);
        }

        if (player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    private void wallJump(InputAction.CallbackContext obj)
    {
        stateMachine.ChangeState(player.wallJump);
    }
}
