using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_DeathBringerTrigger : Enemy_AnimationTriggers
{
    private Enemy_DeathBringer enemyDeathBringer => GetComponentInParent<Enemy_DeathBringer>();

    private void Relocate()
    {
        if(!enemyDeathBringer.CheckSpellCastCooldown() && Random.Range(0,100)<50)//技能不在冷却则执行随机传送,并且每次有50%概率释放技能
        {
            enemyDeathBringer.FindPosition();
        }
        else
        {
            enemyDeathBringer.FindPlayerPosition();
        }
    }

    private void MakeInvisible() => enemyDeathBringer.fx?.MakeTransprent(true);
    private void MakeVisible() => enemyDeathBringer.fx?.MakeTransprent(false);

    private void EnterArea()
    {
        enemy.stateMachine.ChangeState((enemy as Enemy_DeathBringer).idleState);
    }
}
