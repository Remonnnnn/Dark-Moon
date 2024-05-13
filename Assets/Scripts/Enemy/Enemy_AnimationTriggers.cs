using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AnimationTriggers : MonoBehaviour
{
    public Enemy enemy=>GetComponentInParent<Enemy>();

    private void AnimationTriggers()
    {
        enemy.AnimationFinishTriggers();
    }

    protected virtual void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);

        foreach(var hit in colliders)
        {
            if(hit.GetComponent<Player>()!=null)
            {
                PlayerStats target=hit.GetComponent<PlayerStats>();
                enemy.stats.DoDamage(target);
            }
        }

        for (int i = 0; i < enemy.attackSFXId.Length; i++)
        {
            AudioManager.instance.PlaySFX(enemy.attackSFXId[i], enemy.transform);
        }
    }

    private void SpecialAttackTrigger()
    {
        enemy.AnimationSpecialTrigger();
    }

    protected void OpenCounterWindow()=>enemy.OpenCounterAttackWindow();
    protected void CloseCounterWindow()=>enemy.CloseCounterAttackWindow();
}
