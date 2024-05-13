using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        InputManager.instance.inputControl.GamePlay.Jump.started += Jump;
        InputManager.instance.inputControl.GamePlay.Attack.started += Attack;
        InputManager.instance.inputControl.GamePlay.CounterAttack.started += CounterAttack;
        InputManager.instance.inputControl.GamePlay.Blackhole.started += Blackhole;
        InputManager.instance.inputControl.GamePlay.Sword.started += Sword;
    }

    public override void Exit()
    {
        base.Exit();

        InputManager.instance.inputControl.GamePlay.Jump.started -= Jump;
        InputManager.instance.inputControl.GamePlay.Attack.started -= Attack;
        InputManager.instance.inputControl.GamePlay.CounterAttack.started -= CounterAttack;
        InputManager.instance.inputControl.GamePlay.Blackhole.started-=Blackhole;
        InputManager.instance.inputControl.GamePlay.Sword.started -= Sword;

    }

    public override void Update()
    {
        base.Update();



        if (!player.IsGroundDetected())//实时检测Player的接地状态
        {
            stateMachine.ChangeState(player.airState);
        }
    }

    private bool HasNoSword()
    {
        if(!player.sword)
        {
            return true;
        }

        player.sword.GetComponent<Sword_Skill_Controller>().ReturnSword();
        return false;
    }

    #region input event
    private void Jump(InputAction.CallbackContext obj)
    {
        if (player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.jumpState);
        }
    }

    private void Attack(InputAction.CallbackContext obj)
    {
        stateMachine.ChangeState(player.primaryAttack);
    }

    private void CounterAttack(InputAction.CallbackContext obj)
    {
        if (player.skill.parry.parryUnlocked && SkillManager.instance.parry.CanUseSkill())
        {
            stateMachine.ChangeState(player.counterAttack);
            player.ui_InGame.SetCooldownOfParry();
        }
    }

    private void Blackhole(InputAction.CallbackContext obj)
    {
        if (player.skill.blackhole.blackholeUnlocked && !player.skill.blackhole.isCooldown())
        {
            stateMachine.ChangeState(player.blackhole);
            player.ui_InGame.SetCooldownOfBlackhole();
        }
    }

    private void Sword(InputAction.CallbackContext obj)
    {

        if (HasNoSword() && player.skill.sword.swordUnlocked)
        {
            stateMachine.ChangeState(player.aimSword);
        }
    }
    #endregion

   
}
