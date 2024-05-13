using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int possibleItemDrop;//能够掉落的物品数量上限
    [SerializeField] private ItemData[] possibleDrop;//能掉落的物品列表
    [SerializeField] private int[] possibleDropChance;//每种物品的掉率
    private List<ItemData> dropList=new List<ItemData>();

    [SerializeField] private GameObject dropPrefab;

    public virtual void GenerateDrop()
    {
        for(int i=0;i<possibleDropChance.Length;i++)
        {
            if (Random.Range(0, 100) <= possibleDropChance[i])
            {
                dropList.Add(possibleDrop[i]);
            }
        }

        for(int i=0;i<possibleItemDrop;i++)
        {
            if(dropList.Count==0)//当列表物品全部掉落时
            {
                break;
            }

            ItemData randomItem = dropList[Random.Range(0,dropList.Count-1)];

            DropItem(randomItem);
            dropList.Remove(randomItem);
        }
    }

    protected void DropItem(ItemData _itemData)
    {
        GameObject newDrop=Instantiate(dropPrefab,transform.position,Quaternion.identity);

        Vector2 randomvelocity = new Vector2(Random.Range(-5, 5), Random.Range(12, 15));

        newDrop.GetComponent<ItemObject>().SetupItem(_itemData, randomvelocity);
    }
}
