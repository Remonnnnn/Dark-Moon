using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Freeze Enemies Effect", menuName = "Data/Item Effect/Freeze Enemies")]
public class FreezeEnemies_Effect : ItemEffect
{
    [SerializeField] private float duration;

    public override void ExecuteEffect(Transform _transform)
    {
        PlayerStats playerStats=PlayerManager.instance.player.GetComponent<PlayerStats>();

        if(playerStats.currentHealth>playerStats.GetMaxHealthValue()*.1f)//在生命小于10%时触发
        {
            return;
        }

        if(!Inventory.instance.CanUseArmor())//判定能否使用技能
        {
            return;
        }
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_transform.position, 2);//在2的范围内停止时间

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Enemy>().FreezeTimeFor(duration);
            }
        }
    }
}
