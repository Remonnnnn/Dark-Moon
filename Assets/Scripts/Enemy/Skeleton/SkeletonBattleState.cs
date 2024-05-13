using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private Transform player;
    private Enemy_Skeleton enemy;
    private int moveDir;
    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName,Enemy_Skeleton _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;

        if(player.GetComponent<PlayerStats>().isDead)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
    }
    public override void Update()
    {
        base.Update();

        if(enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;

            if(CanAttack() && enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                stateMachine.ChangeState(enemy.attackState);
            }
        }
        else
        {
            if(stateTimer<0 || Vector2.Distance(player.transform.position,enemy.transform.position)>10)
            {
                stateMachine.ChangeState(enemy.idleState);
            }
        }





        if(player.position.x>enemy.transform.position.x)
        {
            moveDir = 1;
        }
        else if(player.position.x<enemy.transform.position.x)
        {
            moveDir = -1;
        }

        if (enemy.IsPlayerDetected() && enemy.IsPlayerDetected().distance < enemy.attackDistance - .1f)
        {
            return;
        }

        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool CanAttack()
    {
        if(Time.time>=enemy.lastTimeAttacked+enemy.attackCoolDown)
        {
            enemy.attackCoolDown=Random.Range(enemy.minAttackCoolDown,enemy.maxAttackCoolDown);
            enemy.lastTimeAttacked= Time.time;
            return true;
        }
        return false;
    }

}
