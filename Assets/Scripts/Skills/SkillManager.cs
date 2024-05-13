using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;


    public Dash_Skill dash { get; private set; }
    public Clone_Skill clone { get; private set; }
    public Sword_Skill sword { get; private set; }
    public Blackhole_Skill blackhole { get; private set; }
    public Crystal_Skill crystal { get; private set; }
    public Parry_Skill parry { get; private set; }

    public Dodge_Skill dodge { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        dash=GetComponent<Dash_Skill>();
        clone=GetComponent<Clone_Skill>();
        sword=GetComponent<Sword_Skill>();
        blackhole=GetComponent<Blackhole_Skill>();
        crystal=GetComponent<Crystal_Skill>();
        parry=GetComponent<Parry_Skill>();
        dodge=GetComponent<Dodge_Skill>();
    }

    public void ReLoadSkill()
    {
        dash.InitUnlock();
        clone.InitUnlock();
        sword.InitUnlock();
        blackhole.InitUnlock();
        crystal.InitUnlock();
        parry.InitUnlock();
        dodge.InitUnlock();
    }

    public void CheckSkill()
    {
        dash.CheckUnlock();
        clone.CheckUnlock();
        sword.CheckUnlock();
        blackhole.CheckUnlock();
        crystal.CheckUnlock();
        parry.CheckUnlock();
        dodge.CheckUnlock();
    }
}
