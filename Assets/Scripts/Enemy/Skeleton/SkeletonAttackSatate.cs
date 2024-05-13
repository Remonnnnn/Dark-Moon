using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttackSatate : EnemyState
{
    private Enemy_Skeleton enemy;
    public SkeletonAttackSatate(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName,Enemy_Skeleton _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

        enemy.lastTimeAttacked=Time.time;
    }

    public override void Update()
    {
        base.Update();

        enemy.SetZeroVelocity();


        if(triggerCalled)
        {
            stateMachine.ChangeState(enemy.battleState);
        }

    }
}
