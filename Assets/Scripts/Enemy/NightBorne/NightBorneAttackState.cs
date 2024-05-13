using System.Collections;
using UnityEngine;


public class NightBorneAttackState : EnemyState
{
    public Enemy_NightBorne enemy;
    public NightBorneAttackState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Enemy_NightBorne _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.isAnim = true;
    }

    public override void Exit()
    {
        base.Exit();
        stateMachine.isAnim = false;
    }

    public override void Update()
    {
        base.Update();

        enemy.SetZeroVelocity();
        
        if(triggerCalled)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}