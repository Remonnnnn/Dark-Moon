using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeDeadState : EnemyState
{
    private Enemy_Slime enemy;
    public SlimeDeadState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName,Enemy_Slime _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
    {
        this.enemy= _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        //enemy.anim.SetBool(enemy.lastAnimBoolName, true);
        enemy.SetZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }


}
