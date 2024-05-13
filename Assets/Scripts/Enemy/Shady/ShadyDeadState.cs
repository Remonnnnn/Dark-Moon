using System.Collections;
using UnityEngine;

public class ShadyDeadState : EnemyState
{
    private Enemy_Shady enemy;
    public ShadyDeadState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName,Enemy_Shady _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
    {
        this.enemy = _enemy;
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

        if(triggerCalled)
        {
            enemy.SelfDestroy();
        }
    }
}