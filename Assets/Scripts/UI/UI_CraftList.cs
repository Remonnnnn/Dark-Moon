using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_CraftList : MonoBehaviour
{
    [SerializeField] private Transform craftSlotParent;
    [SerializeField] private GameObject craftSloPrefab;

    [SerializeField] private List<ItemData_Equipment> craftEquipment;

    private void Start()
    {
        transform.parent.GetChild(0).GetComponent<UI_CraftList>().SetupCraftList();//��Ĭ�Ͽ�������Ʒ�б�����Ϊ��һ��
        SetupDefaultCraftWindow();//����Ĭ����������
        GetComponent<Button>().onClick.AddListener(() => OnPointerDown());
    }

    public void SetupCraftList()
    {
        for(int i=0;i<craftSlotParent.childCount;i++)
        {
            Destroy(craftSlotParent.GetChild(i).gameObject);
        }

        for(int i=0;i<craftEquipment.Count;i++)
        {
            GameObject newSlot = Instantiate(craftSloPrefab, craftSlotParent);
            newSlot.GetComponent<UI_CraftSlot>().SetupCraftSlot(craftEquipment[i]);
        }
    }

    public void OnPointerDown()
    {
        SetupCraftList();
    }

    public void SetupDefaultCraftWindow()
    {
        if (craftEquipment[0]!=null)
        {
            GetComponentInParent<UI>().craftWindow.SetupCraftWindow(craftEquipment[0]);
        }
    }
}
