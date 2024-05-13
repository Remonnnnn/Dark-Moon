using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dash_Skill : Skill
{
    [Header("Dash")]
    [SerializeField] private UI_SkillTreeSlot dashUnlockButton;
    public bool dashUnlocked { get; private set; }

    [Header("Clone on dash")]
    [SerializeField] private UI_SkillTreeSlot cloneOnDashUnlockButton;
    public bool cloneOnDashUnlocked { get;private set; }

    [Header("Clone on arrival")]
    [SerializeField] private UI_SkillTreeSlot cloneOnArrivalUnlockButton;
    public bool cloneOnArrivalUnlocked { get; private set; }
    public override void UseSkill()
    {
        base.UseSkill();

    }

    protected override void Start()
    {
        base.Start();

        dashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockDash);
        cloneOnDashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnDash);
        cloneOnArrivalUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnArrival);
    }


    //���ܽ�������
    public override void CheckUnlock()
    {
        UnlockDash();
        UnlockCloneOnDash();
        UnlockCloneOnArrival();
    }
    private void UnlockDash()
    {
        if(dashUnlockButton.unlocked)
        {
            dashUnlocked = true;
            UI_inGame_Image.SetActive(true);
        }
    }
    private void UnlockCloneOnDash()
    {
        if (cloneOnDashUnlockButton.unlocked)
        {
            cloneOnDashUnlocked = true;
        }
    }
    private void UnlockCloneOnArrival()
    {
        if(cloneOnArrivalUnlockButton.unlocked)
        {
            cloneOnArrivalUnlocked = true;
        }
    }

    public override void InitUnlock()
    {
        dashUnlocked= false;
        cloneOnArrivalUnlocked= false;
        cloneOnDashUnlocked= false;

        UI_inGame_Image.SetActive(false);

        dashUnlockButton.InitSkiil();
        cloneOnDashUnlockButton.InitSkiil();
        cloneOnDashUnlockButton.InitSkiil();

    }



    public void CloneOnDash()//�ڳ����㴴��һ������
    {
        if (cloneOnDashUnlocked)
        {
            SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
        }
    }

    public void CloneOnArrival()//�ڳ���յ㴴��һ������
    {
        if (cloneOnArrivalUnlocked)
        {
            SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
        }
    }

    
}
