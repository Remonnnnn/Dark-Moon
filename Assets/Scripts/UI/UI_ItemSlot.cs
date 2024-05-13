using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UI_ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemText;

    private Color defaultColor;

    protected UI ui;
    public InventoryItem item;

    protected void Awake()
    {
        defaultColor = Color.clear;
    }

    protected virtual void Start()
    {
        ui=GetComponentInParent<UI>();
        GetComponent<Button>().onClick.AddListener(() => OnPointerDown());
    }
    public void UpdateSlot(InventoryItem _newitem)//更新物品格子UI
    {
        item= _newitem;

        itemImage.color = Color.white;

        if (item != null)
        {
            itemImage.sprite = item.data.itemIcon;

            if (item.stackSize > 1)
            {
                itemText.text = item.stackSize.ToString();
            }
            else
            {
                itemText.text = "";
            }
        }

        ButtonCheck();
    }

    public void CleanUpSlot()
    {
        item = null;

        itemImage.color = defaultColor;
        itemText.text = "";

        ButtonCheck();
    }


    public virtual void OnPointerDown()
    {
        if (item == null)
        {
            return;
        }
        //if (Input.GetKey(KeyCode.LeftControl))
        //{
        //    Inventory.instance.RemoveItem(item.data);
        //    return;
        //}
        if (item.data.itemType == ItemType.Equipment)
        {
            Inventory.instance.EquipItem(item.data);
        }

        ui.itemToolTip.HideToolTip();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(item==null)
        {
            return;
        }
        //鼠标管理
        //Vector2 mousePosition = Input.mousePosition;

        //float xOffset = 0;
        //float yOffset = 0;

        //if (mousePosition.x > 700)
        //{
        //    xOffset = -150;
        //}
        //else
        //{
        //    xOffset = 150;
        //}
        //if (mousePosition.y > 700)
        //{
        //    yOffset = -150;
        //}
        //else
        //{
        //    yOffset = 150;
        //}

        //ui.itemToolTip.transform.position=new Vector2(mousePosition.x+xOffset,mousePosition.y+yOffset);
        ui.itemToolTip.ShowToolTip(item.data as ItemData_Equipment);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.itemToolTip.HideToolTip();
    }

    public void ButtonCheck()
    {
        if(item == null && SceneLoader.instance.currentLoadedScene.sceneType!=SceneType.Menu)
        {
            EventSystem.current.SetSelectedGameObject(GetComponent<Button>().FindSelectableOnLeft()?.gameObject);
            GetComponent<Button>().interactable = false;
        }
        else
        {
            GetComponent<Button>().interactable= true;
        }

    }
}
