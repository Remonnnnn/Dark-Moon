using System.Collections;
using UnityEngine;


public class ArcherAttackState : EnemyState
{
    private Enemy_Archer enemy;
    public ArcherAttackState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Enemy_Archer enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
    {
        this.enemy = enemy;
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