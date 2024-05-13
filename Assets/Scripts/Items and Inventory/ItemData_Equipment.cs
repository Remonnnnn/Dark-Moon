using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Flask
}


[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;

    [Header("Unique effect")]
    public float itemCooldown;
    public ItemEffect[] itemEffects;



    [Header("Major stats")]//人物属性
    public int strength;//提高1点伤害和1%暴击伤害
    public int agility;//提高1%暴击率和1%闪避率
    public int intelligence;//提高1点魔法伤害与1点魔法抵抗
    public int vitality;//提高5点最大生命值

    [Header("Offensive stats")]//攻击
    public int damage;
    public int critChance;
    public int critPower;//暴击伤害加成倍率

    [Header("Defence stats")]//防御
    public int health;
    public int armor;
    public int evasion;
    public int magicResistance;

    [Header("Magic stats")]//魔法
    public int fireDamage;
    public int iceDamage;
    public int lightDamage;

    [Header("Craft requirements")]
    public List<InventoryItem> craftingMaterials;

    private int descriptionLength;

    public void Effect(Transform _enemyPosition)//有目标
    {
        foreach (var item in itemEffects)
        {
            item.ExecuteEffect(_enemyPosition);
        }
    }

    public void Effect()//无目标
    {
        foreach(var item in itemEffects)
        {
            item.ExecuteEffect();
        }
    }
    public void AddModifiers()
    {
        PlayerStats playerStats=PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.AddModifier(strength);
        playerStats.agility.AddModifier(agility);
        playerStats.intelligence.AddModifier(intelligence);
        playerStats.vitality.AddModifier(vitality);

        playerStats.damage.AddModifier(damage); 
        playerStats.critChance.AddModifier(critChance);
        playerStats.critPower.AddModifier(critPower);

        playerStats.maxHealth.AddModifier(health);
        playerStats.armor.AddModifier(armor);
        playerStats.evasion.AddModifier(evasion);
        playerStats.magicResistance.AddModifier(magicResistance);

        playerStats.fireDamage.AddModifier(fireDamage);
        playerStats.iceDamage.AddModifier(iceDamage);
        playerStats.lightDamage.AddModifier(lightDamage);

    }

    public void RemoveModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.RemoveModifier(strength);
        playerStats.agility.RemoveModifier(agility);
        playerStats.intelligence.RemoveModifier(intelligence);
        playerStats.vitality.RemoveModifier(vitality);

        playerStats.damage.RemoveModifier(damage);
        playerStats.critChance.RemoveModifier(critChance);
        playerStats.critPower.RemoveModifier(critPower);

        playerStats.maxHealth.RemoveModifier(health);
        playerStats.armor.RemoveModifier(armor);
        playerStats.evasion.RemoveModifier(evasion);
        playerStats.magicResistance.RemoveModifier(magicResistance);

        playerStats.fireDamage.RemoveModifier(fireDamage);
        playerStats.iceDamage.RemoveModifier(iceDamage);
        playerStats.lightDamage.RemoveModifier(lightDamage);

        playerStats.currentHealth = Mathf.Min(playerStats.currentHealth, playerStats.GetMaxHealthValue());
    }

    public override string GetDescription()
    {
        sb.Length = 0;
        descriptionLength = 0;

        AddItemDescription(strength, "Strength");
        AddItemDescription(agility, "Agility");
        AddItemDescription(intelligence, "Intelligence");
        AddItemDescription(vitality, "Vitality");

        AddItemDescription(damage, "Damage");
        AddItemDescription(critChance, "Crit Chance");
        AddItemDescription(critPower, "Crit Power");

        AddItemDescription(health, "MaxHealth");
        AddItemDescription(evasion, "Evasion");
        AddItemDescription(armor, "Armor");
        AddItemDescription(magicResistance, "Magic Resist");

        AddItemDescription(fireDamage, "FireDamage");
        AddItemDescription(iceDamage, "IceDamage");
        AddItemDescription(lightDamage, "LightDamage");

        sb.AppendLine();
        sb.Append("");

        for(int i=0;i<itemEffects.Length;i++)
        {
            if (itemEffects[i].effectDescription.Length>0)
            {
                sb.AppendLine();
                sb.Append("Unique: " + itemEffects[i].effectDescription);
            }
        }

        sb.AppendLine();//空两格给描述
        sb.Append("");
        sb.AppendLine();
        sb.Append("");

        return sb.ToString();
    }

    private void AddItemDescription(int _value,string _name)
    {
        if(_value!=0)
        {
            if(sb.Length>0)
            {
                sb.AppendLine();
            }
            if(_value>0)
            {
                sb.Append("+ " + _value + " " + _name);
            }

            descriptionLength++;
        }
    }
}
