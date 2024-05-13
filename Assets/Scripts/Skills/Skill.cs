using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public GameObject UI_inGame_Image;

    public float cooldown;
    public float cooldownTimer;

    protected Player player;


    private void Awake()
    {
        player = PlayerManager.instance.player;
    }
    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        cooldownTimer-=Time.deltaTime;
    }

    public virtual void CheckUnlock()
    {

    }

    public virtual void InitUnlock()
    {

    }

    public bool isCooldown()//ֻ�жϼ�����ȴ
    {
        if (cooldownTimer < 0)
        {
            return false;
        }

        //�Ƿ���ʾcooldown
        //player.fx.CreatePopUpText("Cooldown",Color.white);
        return true;
    }
    public virtual bool CanUseSkill()//�жϼ�����ȴ��ʹ��
    {
        if(cooldownTimer<0)
        {
            UseSkill();
            cooldownTimer = cooldown;
            return true;
        }

        player.fx.CreatePopUpText("Cooldown",Color.white);
        //player.fx.CreatePopUpText("Cooldown",Color.white);
        return false;
    }

    public virtual void UseSkill()
    {

    }

    protected virtual Transform FindClosetEnemty(Transform _checkTransform)//�����ҵ���������ĵ���
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_checkTransform.position, 25);

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector2.Distance(_checkTransform.position, hit.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }

        return closestEnemy;
    }

}
