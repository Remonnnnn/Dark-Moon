using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_DeathBringerTrigger : Enemy_AnimationTriggers
{
    private Enemy_DeathBringer enemyDeathBringer => GetComponentInParent<Enemy_DeathBringer>();

    private void Relocate()
    {
        if(!enemyDeathBringer.CheckSpellCastCooldown() && Random.Range(0,100)<50)//���ܲ�����ȴ��ִ���������,����ÿ����50%�����ͷż���
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
