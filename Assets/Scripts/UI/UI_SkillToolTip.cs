using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_SkillToolTip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI skillDescription;
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillPrice;
    [SerializeField] private float defaultNameFontSize;

    public void ShowToolTip(string _skillDescription,string _skillName,string _skillPrice)
    {
        skillDescription.text = _skillDescription;
        skillName.text = _skillName;
        if(_skillPrice==null)
        {
            skillPrice.gameObject.SetActive(false);
        }
        else
        {
            skillPrice.text = _skillPrice;
            skillPrice.gameObject.SetActive(true);
        }

        //AdjustPosition();

        //AdjustFontSize(skillName);

        gameObject.SetActive(true);
    }

    public void HideToolTip()
    {
        skillName.fontSize = defaultNameFontSize;
        skillPrice.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
