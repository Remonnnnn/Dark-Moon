using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public enum StatType
{
    strength,
    agility,
    intelligence,
    vitality,
    damage,
    critChance,
    critPower,
    health,
    armor,
    evasion,
    magicResistance,
    fireDamage,
    iceDamage,
    lightDamage
}
public class CharacterStats : MonoBehaviour
{
    private EntityFX fx;

    [Header("Major stats")]//人物属性
    public Stats strength;//提高1点伤害和1%暴击伤害
    public Stats agility;//提高1%暴击率和1%闪避率
    public Stats intelligence;//提高1点魔法伤害与1点魔法抵抗
    public Stats vitality;//提高5点最大生命值

    [Header("Offensive stats")]//攻击
    public Stats damage;
    public Stats critChance;
    public Stats critPower;//暴击伤害加成倍率

    [Header("Defence stats")]//防御
    public Stats maxHealth;
    public Stats armor;
    public Stats evasion;//闪避率
    public Stats magicResistance;

    [Header("Magic stats")]//魔法
    public Stats fireDamage;
    public Stats iceDamage;
    public Stats lightDamage;

    public float ailmentDuration = 4f;//异常状态时间
    public bool isIgnited;//点燃：持续受到伤害
    public bool isChilled;//冻伤：减少20%护甲和20%移速
    public bool isShocked;//触电：降低20%命中率

    private float ignitedTimer;
    private float igniteDamageCooldown = 1f;
    private float ignitedDamageTimer;
    private int igniteDamage;

    private float chilledTimer;

    private float shockedTimer;
    [SerializeField] private GameObject shockStrikePrefab;
    private int shockDamage;
    public int currentHealth;

    public System.Action onHealthChanged;//血量ui事件
    public System.Action DieEvent;

    public bool isDead;

    public bool isInvincible { get; private set; }
    public bool isVulnerable;


    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);
        currentHealth = GetMaxHealthValue();

        fx = GetComponent<EntityFX>();
    }

    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        ignitedDamageTimer -= Time.deltaTime;

        chilledTimer -= Time.deltaTime;

        shockedTimer -= Time.deltaTime;


        if (ignitedTimer < 0)
        {
            isIgnited = false;
        }
        if (chilledTimer < 0)
        {
            isChilled = false;
        }
        if (shockedTimer < 0)
        {
            isShocked = false;
        }

        if (isIgnited)
        {
            ApplyIgniteDamage();
        }
    }


    public void MakeVulnerableFor(float _duration)
    {
        StartCoroutine(VulnerableForCorutine(_duration));
    }
    private IEnumerator VulnerableForCorutine(float _duration)
    {
        isVulnerable= true;

        yield return new WaitForSeconds(_duration);

        isVulnerable = false;

    }
    public virtual void IncreaseStatBy(int _modifier, float _duration, Stats _staToModify)
    {
        StartCoroutine(StatModCoroutine(_modifier, _duration, _staToModify));
    }

    private IEnumerator StatModCoroutine(int _modifier, float _duration, Stats _staToModify)
    {
        _staToModify.AddModifier(_modifier);

        yield return new WaitForSeconds(_duration);

        _staToModify.RemoveModifier(_modifier);
    }
    public virtual void DoDamage(CharacterStats _targetStats)
    {
        bool criticalStrike = false;

        if (_targetStats.isInvincible)//无敌则返回
        {
            return;
        }

        if (TargetCanAvoidAttack(_targetStats))//闪避判定
        {
            return;
        }

        _targetStats.GetComponent<Entity>().SetupKnockbackDir(transform);

        int totalDamge = damage.GetValue() + strength.GetValue();//攻击者伤害计算

        if (CanCrit())//暴击伤害计算
        {
            totalDamge = CaculateCriticalDamage(totalDamge);
            criticalStrike= true;
        }

        fx.CreateHitFx(_targetStats.transform,criticalStrike);

        totalDamge = CheckTargetArmor(_targetStats, totalDamge);//防御者防御计算

        _targetStats.TakeDamage(totalDamge,Color.red);

        DoMagicalDamage(_targetStats);
    }

    #region Magical damage and Ailments
    public virtual void DoMagicalDamage(CharacterStats _targetStats)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightDamage = lightDamage.GetValue();

        int totalMagicDamage = _fireDamage + _iceDamage + _lightDamage + intelligence.GetValue();

        totalMagicDamage = CheckTargetResistance(_targetStats, totalMagicDamage);//计算目标法抗
        _targetStats.TakeDamage(totalMagicDamage,Color.blue);

        if (Mathf.Max(_fireDamage, _iceDamage, _lightDamage) <= 0)
        {
            return;
        }

        AttemptToApplyAilments(_targetStats, _fireDamage, _iceDamage, _lightDamage);
    }

    private  void AttemptToApplyAilments(CharacterStats _targetStats, int _fireDamage, int _iceDamage, int _lightDamage)
    {
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightDamage;
        bool canApplyShock = _lightDamage > _fireDamage && _lightDamage > _iceDamage;

        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            int v = Random.Range(1, 4);
            if (v == 1 && _fireDamage > 0)
            {
                canApplyIgnite = true;
                break;
            }
            if (v == 2 && _iceDamage > 0)
            {
                canApplyChill = true;
                break;
            }
            if (v == 3 && _lightDamage > 0)
            {
                canApplyShock = true;
                break;
            }
        }

        if (canApplyIgnite)
        {
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(Mathf.Max(1, _fireDamage * .2f)));
        }
        if (canApplyShock)
        {
            _targetStats.SetupShockStrikeDamage(Mathf.RoundToInt(Mathf.Max(1, _lightDamage * .1f)));
        }

        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }

    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
        bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
        bool canApplyChill = !isIgnited && !isChilled && !isShocked;
        bool canApplyShock = !isIgnited && !isChilled;

        if (_ignite && canApplyIgnite)//点燃
        {
            isIgnited = _ignite;
            ignitedTimer = ailmentDuration;

            fx.IgniteFxFor(ailmentDuration);
        }
        if(_chill && canApplyChill)//冰冻
        {
            isChilled = _chill;
            chilledTimer = ailmentDuration;

            float slowPercentage = .2f;//减速百分比

            GetComponent<Entity>().SlowEntityBy(slowPercentage, ailmentDuration);
            fx.ChillFxFor(ailmentDuration);
        }
        if(_shock && canApplyShock)//触电
        {
            if (!isShocked)
            {
                ApplyShock(_shock);
            }
            else//触电下再次被攻击。将会传导一次闪电
            {

                if (GetComponent<Player>() != null)//如果是玩家则取消
                {
                    return;
                }
                HitNearestTargetWithShockStrike();
            }

        }
    }

    public void ApplyShock(bool _shock)
    {
        if(isShocked)
        {
            return;
        }

        isShocked = _shock;
        shockedTimer = ailmentDuration;

        fx.ShockFxFor(ailmentDuration);
    }//施加触电buff

    private void HitNearestTargetWithShockStrike()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<CharacterStats>() != this && hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }

        if (closestEnemy == null)//周围没有敌人时，闪电攻击自己
        {
            closestEnemy = transform;
        }

        if (closestEnemy != null)
        {
            GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);

            newShockStrike.GetComponent<ShockStrike_Controller>().Setup(shockDamage, closestEnemy.GetComponent<CharacterStats>());

        }
    }//闪电传导攻击

    private void ApplyIgniteDamage()
    {
        if (ignitedDamageTimer < 0)
        {
            DecreaseHealthBy(igniteDamage,Color.blue);

            if (currentHealth <= 0 && !isDead)
            {
                Die();
            }

            ignitedDamageTimer = igniteDamageCooldown;
        }
    }
    public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;
    public void SetupShockStrikeDamage(int _damage)=>shockDamage = _damage;

    #endregion
    public virtual void TakeDamage(int _damage,Color _color)
    {
        if(isInvincible)
        {
            return;
        }
        DecreaseHealthBy(_damage,_color);

        GetComponent<Entity>().DamageImpact();
        fx.StartCoroutine("FlashFX");

        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    public virtual void IncreaseHealthBy(int _amount)
    {
        currentHealth = Mathf.Min(currentHealth + _amount, GetMaxHealthValue());//不能超过最大生命

        if(onHealthChanged!=null)
        {
            onHealthChanged();
        }
    }
    public virtual void DecreaseHealthBy(int _damage,Color _color)
    {
        if (isInvincible)
        {
            return;
        }
        if (isVulnerable)//易伤状态下受到的伤害提高10%
        {
            _damage = Mathf.RoundToInt(_damage * 1.1f);
        }

        currentHealth -= _damage;

        if(_damage>0)
        {
            fx.CreatePopUpText(_damage.ToString(),_color);
        }

        if(onHealthChanged!= null)
        {
            onHealthChanged();
        }

        if(currentHealth<=0 && !isDead)//用于某些情况下，判断血量（反击。。）
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        isDead = true;
    }

    public void killEntity()
    {
        if(!isDead)
        {
            Die();
        }
    }

    public void MakeInvincible(bool _invincible) => isInvincible = _invincible;

    #region Stat calculations

    public virtual void OnEvasion()
    {

    }
    protected bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if(isShocked)//判断当前攻击者是否触电
        {
            totalEvasion += 20;
        }

        if (Random.Range(0, 100) < totalEvasion)//闪避判定
        {
            _targetStats.OnEvasion();
            return true;
        }

        return false;
    }
    protected int CheckTargetArmor(CharacterStats _targetStats, int totalDamge)//防御判断
    {
        if(_targetStats.isChilled)//判断敌人是否被冻伤
        {
            totalDamge -= Mathf.RoundToInt(_targetStats.armor.GetValue() * .8f);
        }
        else
        {
            totalDamge -= _targetStats.armor.GetValue();
        }
        totalDamge = Mathf.Clamp(totalDamge, 0, int.MaxValue);
        return totalDamge;
    }
    private int CheckTargetResistance(CharacterStats _targetStats, int totalMagicDamage)//法抗判断
    {
        totalMagicDamage -= (_targetStats.magicResistance.GetValue() + _targetStats.intelligence.GetValue());
        totalMagicDamage = Mathf.Clamp(totalMagicDamage, 0, int.MaxValue);//计算魔法伤害
        return totalMagicDamage;
    }
    protected bool CanCrit()
    {
        int totalCriticalChance = critChance.GetValue() + agility.GetValue();

        if(Random.Range(0,100)<=totalCriticalChance)
        {
            return true;
        }
        return false;
    }
    protected int CaculateCriticalDamage(int _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * .01f;

        float critDamage = _damage * totalCritPower;

        return Mathf.RoundToInt(critDamage);
    }
    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue()+vitality.GetValue()*5;
    }

    #endregion

    public Stats GetStat(StatType _buffType)
    {
        if (_buffType == StatType.strength)
        {
            return strength;
        }
        else if (_buffType == StatType.agility)
        {
            return agility;
        }
        else if (_buffType == StatType.intelligence)
        {
            return intelligence;
        }
        else if (_buffType == StatType.vitality)
        {
            return vitality;
        }
        else if (_buffType == StatType.damage)
        {
            return damage;
        }
        else if (_buffType == StatType.critChance)
        {
            return critChance;
        }
        else if (_buffType == StatType.critPower)
        {
            return critPower;
        }
        else if (_buffType == StatType.health)
        {
            return maxHealth;
        }
        else if (_buffType == StatType.armor)
        {
            return  armor;
        }
        else if (_buffType == StatType.evasion)
        {
            return evasion;
        }
        else if (_buffType == StatType.magicResistance)
        {
            return magicResistance;
        }
        else if (_buffType == StatType.fireDamage)
        {
            return fireDamage;
        }
        else if (_buffType == StatType.iceDamage)
        {
            return iceDamage;
        }
        else if (_buffType == StatType.lightDamage)
        {
            return lightDamage;
        }

        return null;
    }

    public void SetLoadStats()
    {
        Debug.Log("success load Player");
        isDead = false;
        MakeInvincible(false);
        currentHealth = GetMaxHealthValue();
        onHealthChanged();
    }
}

