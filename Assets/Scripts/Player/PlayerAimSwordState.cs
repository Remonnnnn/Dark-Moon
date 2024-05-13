using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAimSwordState : PlayerState
{
    public PlayerAimSwordState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        InputManager.instance.inputControl.GamePlay.Sword.canceled += ReturnToIdle;
        player.skill.sword.DotsActive(true);
    }

    public override void Exit()
    {
        base.Exit();

        InputManager.instance.inputControl.GamePlay.Sword.canceled -= ReturnToIdle;
        player.StartCoroutine("BusyFor",.2f);
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();

        //if(Input.GetKeyUp(KeyCode.Mouse1))
        //{
        //    stateMachine.ChangeState(player.idleState);
        //}



        if(player.InputDirection.x<0 && player.facingDir==1)
        {
            player.Flip();
        }
        else if(player.InputDirection.x > 0 && player.facingDir == -1)
        {
            player.Flip();
        }
    }

    private void ReturnToIdle(InputAction.CallbackContext obj)
    {
        stateMachine.ChangeState(player.idleState);
    }
}
