using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Equipment : UI_ItemSlot
{
    public EquipmentType slotType;

    private void OnValidate()
    {
        gameObject.name="Equipment slot - "+slotType.ToString();
    }

    public override void OnPointerDown()
    {
        if(item==null || item.data==null)
        {
            return;
        }

        Inventory.instance.UnequiItem(item.data as ItemData_Equipment);
        Inventory.instance.AddItem(item.data as ItemData_Equipment);

        ui.itemToolTip.HideToolTip();

        CleanUpSlot();
    }
}
