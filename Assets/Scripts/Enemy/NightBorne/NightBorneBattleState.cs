using System.Collections;
using UnityEngine;

public class NightBorneBattleState : EnemyState
{
    private Transform player;
    private Enemy_NightBorne enemy;
    int moveDir;
    public NightBorneBattleState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Enemy_NightBorne _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemy.stateMachine.ChangeState(enemy.battleState);

        if (player.position.x > enemy.transform.position.x)
        {
            moveDir = 1;
        }
        else if (player.position.x < enemy.transform.position.x)
        {
            moveDir = -1;
        }


        enemy.SetVelocity(enemy.moveSpeed * moveDir, enemy.rb.velocity.y);
    }
}