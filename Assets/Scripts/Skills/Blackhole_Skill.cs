using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blackhole_Skill : Skill
{

    [SerializeField] private UI_SkillTreeSlot blackholeUnlockButton;
    public bool blackholeUnlocked { get; private set; }
    [SerializeField] private int amountOfAttacks;
    [SerializeField] private float cloneAttackCooldown;
    [SerializeField] private float blackholeDuration;
    [Space]
    [SerializeField] private GameObject blackHolePrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;

    Blackhole_Skill_Controller currentBlackhole;


    //技能解锁区域
    public override void CheckUnlock()
    {
        UnlockBlackhole();
    }
    private void UnlockBlackhole()
    {
        if(blackholeUnlockButton.unlocked)
        {
            blackholeUnlocked = true;
            UI_inGame_Image.SetActive(true);
        }
    }

    public override void InitUnlock()
    {
        blackholeUnlocked = false;

        UI_inGame_Image.SetActive(false);

        blackholeUnlockButton.InitSkiil();
    }



    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        GameObject newBlackhole = Instantiate(blackHolePrefab,player.transform.position,Quaternion.identity);

        currentBlackhole=newBlackhole.GetComponent<Blackhole_Skill_Controller>();

        currentBlackhole.SetupBlackhole(maxSize, growSpeed, shrinkSpeed, amountOfAttacks, cloneAttackCooldown,blackholeDuration);

        AudioManager.instance.PlaySFX(3, player.transform);
        AudioManager.instance.PlaySFX(6, player.transform);
    }

    protected override void Start()
    {
        base.Start();

        blackholeUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBlackhole);
    }

    protected override void Update()
    {
        base.Update();
    }

    public bool SkillCompleted()
    {
        if(!currentBlackhole)
        {
            return false;
        }
        if(currentBlackhole.playerCanExitState)
        {
            currentBlackhole= null;
            AudioManager.instance.StopSFX(3);
            return true;
        }
        return false;
    }

    public float GetBlackholeRadius()
    {
        return maxSize / 2;
    }
}
