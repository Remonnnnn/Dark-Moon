using System.Collections;
using UnityEngine;

public class ShadyAttackState : EnemyState
{
    private Enemy_Shady enemy;
    public ShadyAttackState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Enemy_Shady _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

        enemy.lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        enemy.SetZeroVelocity();


        if (triggerCalled)
        {
            stateMachine.ChangeState(enemy.battleState);
        }

    }
}