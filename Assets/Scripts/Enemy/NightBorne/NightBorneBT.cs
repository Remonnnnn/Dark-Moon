using System.Collections;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using System.Collections.Generic;

using Behavior_Tree;
public class NightBorneBT : Behavior_Tree.Tree
{
    public Player player;
    public Enemy_NightBorne enemy;//此处的enemy相当于黑板节点，传递一个敌人的所有信息
    protected override void Awake()
    {
        player=PlayerManager.instance.player;
    }
    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new ConditionNode(CheckFightNotBegun),
            new Sequence(new List<Node>
            {
                new ConditionNode(CheckPlayerInAttackRange),
                new ActionNode(Attack)
            }),
            new Sequence(new List<Node>
            {
                new ConditionNode(CheckIsNotAnim),
                new ActionNode(Battle)
            }),
        });;

        return root;
    }

    public bool CheckFightNotBegun()
    {
        return !enemy.bossFightBegun;
    }
    public bool CheckPlayerInAttackRange()
    {
        Debug.Log("Check Player In AttackRange");
        return enemy.IsPlayerDetected();
    }

    public bool CheckIsNotAnim()
    {
        Debug.Log("Check Is Anim");
        return !enemy.stateMachine.CheckIsAnim();
    }

    public NodeState Battle()
    {
        if(CheckNeedBattle())
        {
            enemy.stateMachine.ChangeState(enemy.battleState);
        }
        else
        {
            enemy.stateMachine.ChangeState(enemy.idleState);
        }

        return NodeState.Running;
    }

    public NodeState Attack()
    {
        if(CheckIsNotAnim() && CanAttack())
        {
            enemy.stateMachine.ChangeState(enemy.attackState);
        }

        return NodeState.Success;
    }




    private bool CanAttack()
    {
        if (Time.time >= enemy.lastTimeAttacked + enemy.attackCoolDown)
        {
            enemy.attackCoolDown = Random.Range(enemy.minAttackCoolDown, enemy.maxAttackCoolDown);
            enemy.lastTimeAttacked = Time.time;
            return true;
        }
        return false;
    }

    public bool CheckNeedBattle()
    {
        return Vector2.Distance(player.transform.position, enemy.transform.position) > 2;
    }
}