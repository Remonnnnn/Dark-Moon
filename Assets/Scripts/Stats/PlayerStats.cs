using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player player;
    protected override void Start()
    {
        base.Start();

        player = GetComponent<Player>();
    }

    public override void TakeDamage(int _damage, Color _color)
    {
        base.TakeDamage(_damage,_color);
    }

    protected override void Die()
    {
        base.Die();
        player.Die();

        if(DieEvent!= null)//ִ�������¼�
        {
            DieEvent();
        }

        MakeInvincible(true);
        GameManager.instance.lostCurrencyAmount = PlayerManager.instance.currency;//����ʱ��������money
        PlayerManager.instance.currency = 0;

        GetComponent<PlayerItemDrop>()?.GenerateDrop();
    }

    public override void DecreaseHealthBy(int _damage,Color _color)
    {
        base.DecreaseHealthBy(_damage,_color);

        if(_damage>GetMaxHealthValue() * .3f)
        {
            InputManager.instance.SetMotorGamepad(.25f, .25f, .75f);
            player.SetupKnockbackPower(new Vector2(6, 7));
            player.fx.ScreenShake(player.fx.shakeHighDamage);

            int randomSound = Random.Range(34, 35);
            AudioManager.instance.PlaySFX(34, null);
        }

        ItemData_Equipment currentArmor = Inventory.instance.GetEquipment(EquipmentType.Armor);

        if(currentArmor != null)
        {
            currentArmor.Effect(player.transform);
        }
    }

    public override void OnEvasion()
    {
        player.skill.dodge.CreateMirageOnDoDodge();
    }

    public void CloneDoDamge(CharacterStats _targetStats,float _multiplier)//��¡�˺��ж�
    {
        if (TargetCanAvoidAttack(_targetStats))//�����ж�
        {
            return;
        }

        int totalDamage=damage.GetValue()+strength.GetValue();

        if(_multiplier > 0)
        {
            totalDamage = Mathf.Max(1,Mathf.RoundToInt(totalDamage * _multiplier));//�˺�����
        }

        int totalDamge = damage.GetValue() + strength.GetValue();//�������˺�����

        if (CanCrit())//�����˺�����
        {
            totalDamge = CaculateCriticalDamage(totalDamge);
        }

        totalDamge = CheckTargetArmor(_targetStats, totalDamge);//�����߷�������

        _targetStats.TakeDamage(totalDamge,Color.red);

        DoMagicalDamage(_targetStats);
    }



}
