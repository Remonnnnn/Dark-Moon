using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stats
{
    [SerializeField] private int baseValue;

    public List<int> modifiers;
    public int GetValue()//返回最终的数值
    {
        int finalValue = baseValue;

        foreach(int modifier in modifiers)//添加装备的数值
        {
            finalValue += modifier;
        }
        
        return finalValue;
    }

    public void SetDefaultValue(int _value)
    {
        baseValue = _value;
    }
    public void AddModifier(int _modifier)
    {
        modifiers.Add(_modifier);
    }

    public void RemoveModifier(int _modifier)
    {
        modifiers.Remove(_modifier);
    }
}
