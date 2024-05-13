using System.Collections;
using UnityEngine;

public class NightBorneIdleState : EnemyState
{
    private Enemy_NightBorne enemy;
    public NightBorneIdleState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName,Enemy_NightBorne _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
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
    }

    public override void Update()
    {
        base.Update();

    }
}