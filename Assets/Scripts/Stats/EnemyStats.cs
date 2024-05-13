using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;
    private ItemDrop myDropSystem;
    public Stats moneyDropAmount;

    [Header("level details")]
    [SerializeField] private int level;

    [Range(0f, 1f)]//等级加成
    [SerializeField] private float percentageModifier;

    protected override void Start()
    {
        ApplyLevelModifiers();

        base.Start();

        moneyDropAmount.SetDefaultValue(100);
        enemy = GetComponent<Enemy>();
        myDropSystem = GetComponent<ItemDrop>();
    }

    private void ApplyLevelModifiers()
    {
        Modify(damage);
        Modify(critChance);
        Modify(critPower);

        Modify(maxHealth);
        Modify(armor);
        Modify(evasion);
        Modify(magicResistance);

        Modify(fireDamage);
        Modify(iceDamage);
        Modify(lightDamage);

        Modify(moneyDropAmount);
    }

    private void Modify(Stats _stat)
    {
        for(int i=0;i<level;i++)
        {
            float modifier = _stat.GetValue() * percentageModifier;

            _stat.AddModifier(Mathf.RoundToInt(modifier));
        }
    }
    public override void TakeDamage(int _damage,Color _color)
    {
        base.TakeDamage(_damage,_color);

    }

    protected override void Die()
    {
        base.Die();

        enemy.Die();

        PlayerManager.instance.currency += moneyDropAmount.GetValue();
        myDropSystem.GenerateDrop();

        Destroy(gameObject, 1.5f);
    }
}
