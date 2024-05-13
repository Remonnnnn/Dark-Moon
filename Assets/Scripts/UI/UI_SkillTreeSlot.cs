using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillTreeSlot : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,ISaveManager
{
    private UI ui;
    private Image skillImage;

    [SerializeField] private int skillPrice;
    [SerializeField] private string skillName;
    [TextArea]
    [SerializeField]private string skillDescription;
    [SerializeField] private Color lockdeSkillColor;


    
    public bool unlocked;
    private bool isShow;

    [SerializeField] private UI_SkillTreeSlot[] shouldBeUnlocked;
    [SerializeField] private UI_SkillTreeSlot[] shouldBeLocked;



    private void OnValidate()
    {
        gameObject.name = "SkillTreeSlot_UI - " + skillName;
    }

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());

        skillImage = GetComponent<Image>();
        ui = GetComponentInParent<UI>();


    }
    private void Start()
    {
        if(unlocked)
        {
            skillImage.color = Color.white;
        }
        else
        {
            skillImage.color = lockdeSkillColor;
        }

    }

    public void UnlockSkillSlot()
    {
        if(PlayerManager.instance.HaveEnoughMoney(skillPrice)==false)
        {
            return;
        }

        if(CanBeUnlocked())
        {
            unlocked = true;
            skillImage.color = Color.white;
        }
        else
        {
            Debug.Log("Locked Skill");
        }
    }

    public bool CanBeUnlocked()
    {
        for (int i = 0; i < shouldBeUnlocked.Length; i++)
        {
            if (shouldBeUnlocked[i].unlocked == false)
            {
                return false;
            }
        }

        for (int i = 0; i < shouldBeLocked.Length; i++)
        {
            if (shouldBeLocked[i].unlocked == true)
            {
                return false;
            }
        }

        return true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(unlocked)//解锁状态
        {
            ui.skillToolTip.ShowToolTip(skillDescription, skillName, null);
        }
        else if(CanBeUnlocked())//可解锁状态
        {
            ui.skillToolTip.ShowToolTip(skillDescription, skillName,skillPrice.ToString("#,#"));
        }
        else//无法解锁状态
        {
            ui.skillToolTip.ShowToolTip("Locked", "???",null);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.HideToolTip();
    }

    public void LoadData(GameData _data)
    {
        Debug.Log("load skill");
        if(_data.skillTree.TryGetValue(skillName,out bool value))
        {
            unlocked = value;
        }

        if (unlocked)
        {
            skillImage.color = Color.white;
        }
        else
        {
            skillImage.color = lockdeSkillColor;
        }
    }

    public void SaveData(ref GameData _data)
    {
        Debug.Log("save skill");
        if(_data.skillTree.TryGetValue(skillName,out bool value))
        {
            _data.skillTree[skillName] = unlocked;//已存在就直接改变其unlockedz状态
        }
        else
        {
            _data.skillTree.Add(skillName, unlocked);
        }
    }

    public void InitSkiil()
    {
        unlocked = false;
        skillImage.color = lockdeSkillColor;
    }

}
