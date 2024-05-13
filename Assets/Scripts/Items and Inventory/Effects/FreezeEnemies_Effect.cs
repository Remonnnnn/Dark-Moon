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

        if(playerStats.currentHealth>playerStats.GetMaxHealthValue()*.1f)//������С��10%ʱ����
        {
            return;
        }

        if(!Inventory.instance.CanUseArmor())//�ж��ܷ�ʹ�ü���
        {
            return;
        }
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_transform.position, 2);//��2�ķ�Χ��ֹͣʱ��

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Enemy>().FreezeTimeFor(duration);
            }
        }
    }
}
