using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Parry_Skill : Skill
{
    [Header("Parry")]
    [SerializeField] private UI_SkillTreeSlot parryUnlockButton;
    public bool parryUnlocked;

    [Header("Parry restore")]
    [SerializeField] private UI_SkillTreeSlot restoreUnlockButton;
    [Range(0f, 1f)]
    [SerializeField] private float restoreHealthAmount;
    public bool restoreUnlocked { get;private set; }

    [Header("Parry with mirage")]
    [SerializeField] private UI_SkillTreeSlot parryWithMirageUnlockButton;
    public bool parryWithMirageUnlocked { get;private set; }

    public override void UseSkill()
    {
        base.UseSkill();
    }

    protected override void Start()
    {
        base.Start();

        parryUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParry);
        restoreUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryRestore);
        parryWithMirageUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryWithMirage);
    }


    //技能解锁区域
    public override void CheckUnlock()
    {
        UnlockParry();
        UnlockParryRestore();
        UnlockParryWithMirage();
    }
    private void UnlockParry()
    {
        if (parryUnlockButton.unlocked)
        {
            parryUnlocked = true;
            UI_inGame_Image.SetActive(true);
        }
    }
    private void UnlockParryRestore()
    {
        if (restoreUnlockButton.unlocked)
        {
            restoreUnlocked = true;
        }
    }
    private void UnlockParryWithMirage()
    {
        if (parryWithMirageUnlockButton.unlocked)
        {
            parryWithMirageUnlocked= true;
        }
    }

    public override void InitUnlock()
    {
        parryUnlocked= false;
        parryWithMirageUnlocked= false;
        restoreUnlocked=false;

        UI_inGame_Image.SetActive(false);

        parryUnlockButton.InitSkiil();
        parryWithMirageUnlockButton.InitSkiil();
        restoreUnlockButton.InitSkiil();
    }


    public void MakeMirageOnParry(Transform _respawnTransform)
    {
        if(parryWithMirageUnlocked)
        {
            SkillManager.instance.clone.CreateCloneWithDelay(_respawnTransform);
        }
    }

    public void RestoreOnParry()
    {
        if (restoreUnlocked)
        {
            int restoreAmount = Mathf.RoundToInt(Mathf.Max(1, (player.stats.maxHealth.GetValue() * restoreHealthAmount)));//至少回复一点
            player.stats.IncreaseHealthBy(restoreAmount);
        }
    }

}
