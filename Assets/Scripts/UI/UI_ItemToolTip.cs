using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ItemToolTip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private TextMeshProUGUI itemStroyText;

    [SerializeField] private int defaultFontSize = 32;

    public void ShowToolTip(ItemData_Equipment item)
    {
        if (item==null)
        {
            return;
        }
        itemNameText.text = item.itemName;
        itemTypeText.text=item.equipmentType.ToString();
        itemDescription.text = item.GetDescription();
        itemStroyText.text = item.itemStroy;

        if(itemNameText.text.Length>14)
        {
            itemNameText.fontSize = defaultFontSize * .7f;
        }
        else
        {
            itemNameText.fontSize = defaultFontSize;
        }

        gameObject.SetActive(true);
    }

    public void HideToolTip()=>gameObject.SetActive(false);
}
