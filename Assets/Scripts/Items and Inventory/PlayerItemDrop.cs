using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
    [Header("Player's drop")]
    [SerializeField] private float chanceToLoseItems;
    [SerializeField] private float chanceToLoseMaterials;

    public override void GenerateDrop()
    {
        Inventory inventory = Inventory.instance;

        List<InventoryItem>currentEquipment=Inventory.instance.GetEquipmentList();
        List<InventoryItem>currentMaterial=Inventory.instance.GetStashList();

        for(int i=0;i<currentEquipment.Count;i++)//ÎäÆ÷µôÂä
        {
            if(Random.Range(0,100)<=chanceToLoseItems)
            {
                DropItem(currentEquipment[i].data);
                inventory.UnequiItem(currentEquipment[i].data as ItemData_Equipment);
            }
        }

        for(int i=0;i<currentMaterial.Count;i++)//²ÄÁÏµôÂä
        {
            if(Random.Range(0,100)<=chanceToLoseMaterials)
            {
                DropItem(currentMaterial[i].data);
                inventory.RemoveItem(currentMaterial[i].data);
            }
        }
    }
}
