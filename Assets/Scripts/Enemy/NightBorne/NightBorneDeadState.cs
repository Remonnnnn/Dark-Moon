using System.Collections;
using UnityEngine;

public class NightBorneDeadState : EnemyState
{
    private Enemy_NightBorne enemy;
    public NightBorneDeadState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Enemy_NightBorne _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        stateMachine.isAnim=true;
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