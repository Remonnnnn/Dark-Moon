using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dodge_Skill : Skill
{
    [Header("Dodge")]
    [SerializeField] UI_SkillTreeSlot unlockDodgeButton;
    [SerializeField] private int evasionAmount;
    public bool dodgeUnlocked;

    [Header("Mirage dodge")]
    [SerializeField] private UI_SkillTreeSlot unlockMirageDodge;
    public bool dodgeMirageUnlocked;

    protected override void Start()
    {
        base.Start();

        unlockDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockDodge);
        unlockMirageDodge.GetComponent<Button>().onClick.AddListener(UnlockMirageDodge);
    }


    //技能解锁区域
    public override void CheckUnlock()
    {
        UnlockDodge();
        UnlockMirageDodge();
    }
    private void UnlockDodge()
    {
        if(unlockDodgeButton.unlocked && !dodgeUnlocked)
        {
            player.stats.evasion.AddModifier(evasionAmount);
            Inventory.instance.UpdateStasUI();
            dodgeUnlocked = true;
        }
    }
    private void UnlockMirageDodge()
    {
        if(unlockMirageDodge.unlocked)
        {
            dodgeMirageUnlocked = true;
        }
    }

    public override void InitUnlock()
    {
        if(dodgeUnlocked)//被动要移除
        {
            player.stats.evasion.RemoveModifier(evasionAmount);
        }


        dodgeUnlocked = false;
        dodgeMirageUnlocked= false;

        Inventory.instance.UpdateStasUI();

        unlockDodgeButton.InitSkiil();
        unlockMirageDodge.InitSkiil();

    }



    public void CreateMirageOnDoDodge()
    {
        if(dodgeMirageUnlocked)
        {
            SkillManager.instance.clone.CreateClone(player.transform, new Vector3(2*player.facingDir, 0));
        }
    }
}
