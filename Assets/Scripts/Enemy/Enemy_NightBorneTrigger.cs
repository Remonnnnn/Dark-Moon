using System.Collections;
using UnityEngine;


public class Enemy_NightBorneTrigger : Enemy_AnimationTriggers
{
    protected override void AttackTrigger()
    {
        for (int i = 0; i < enemy.attackSFXId.Length; i++)
        {
            AudioManager.instance.PlaySFX(enemy.attackSFXId[i], enemy.transform);
        }
    }
}