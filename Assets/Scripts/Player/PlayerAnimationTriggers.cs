using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();
    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }
    private void AttackTrigger()
    {
        AudioManager.instance.PlaySFX(2,null);//�ӽ���Ч

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach(var hit in colliders)
        {
            if(hit.GetComponent<Enemy>()!=null)
            {
                EnemyStats target=hit.GetComponent<EnemyStats>();

                if (target.isInvincible)//����޵����޷�ִ����Ч
                {
                    continue;
                }

                if (target!=null)
                {
                    player.stats.DoDamage(target);
                    AudioManager.instance.PlaySFX(1, null);
                }


                ItemData_Equipment weaponData1 = Inventory.instance.GetEquipment(EquipmentType.Weapon);//�ж���������Ч��(��Ŀ��)
                if (weaponData1 != null)
                {
                    weaponData1.Effect(target.transform);
                }
            }
        }

        ItemData_Equipment weaponData2 = Inventory.instance.GetEquipment(EquipmentType.Weapon);//�ж���������Ч������Ŀ�꣩��ÿ�ι���������һ��
        if (weaponData2 != null)
        {
            weaponData2.Effect();
        }

    }

    private void ThrowSword()
    {
        SkillManager.instance.sword.CreateSword();
    }
}
