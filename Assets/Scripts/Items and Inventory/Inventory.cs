using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



public class Inventory : MonoBehaviour,ISaveManager
{
    public static Inventory instance;

    public List<ItemData> startingItems;//��ʼװ��

    public List<InventoryItem> equipment;
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;

    public List<InventoryItem> inventory;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;

    public List<InventoryItem> stash;
    private Dictionary<ItemData, InventoryItem> stashDictionary;

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform statSlotParent;

    private UI_ItemSlot[] inventoryItemSlot;
    private UI_ItemSlot[] stashItemSlot;
    private UI_Equipment[] equipmentSlot;
    private UI_StatSlot[] statSlot;

    [Header("Items cooldown")]
    private float lastTimeUsedFlask;
    private float lastTimeUsedArmor;

    public float flaskCooldown { get;private set; }
    private float armorCooldown;

    [Header("Data base")]
    public List<ItemData> itemDataBase;
    public List<InventoryItem> loadedItems;
    public List<ItemData_Equipment> loadedEquipment;
    private void Awake()
    {
        if(instance==null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        inventory = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();

        stash = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();

        equipment = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();

        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        stashItemSlot = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        equipmentSlot = equipmentSlotParent.GetComponentsInChildren<UI_Equipment>();
        statSlot= statSlotParent.GetComponentsInChildren<UI_StatSlot>();

    }

    public void AddLoadItems()
    {
        if(loadedEquipment.Count>0)//��������װ������Ʒ
        {
            foreach(ItemData_Equipment item in loadedEquipment)
            {
                EquipItem(item);
                Debug.Log("equip load item");
            }
        }

        if (loadedItems.Count>0)//������Ʒ
        {
            foreach (InventoryItem item in loadedItems)
            {
                for(int i=0;i<item.stackSize;i++)
                {
                    AddItem(item.data);
                }
            }

            return;
        }
    }

    public void AddStartingItem()
    {
        for (int i = 0; i < startingItems.Count; i++)
        {
            AddItem(startingItems[i]);
        }
    }

    public void EquipItem(ItemData _item)
    {
        ItemData_Equipment newEquipment = _item as ItemData_Equipment;
        InventoryItem newItem = new InventoryItem(newEquipment);

        ItemData_Equipment oldEquipment = null;

        foreach(KeyValuePair<ItemData_Equipment,InventoryItem>item in equipmentDictionary)//Ѱ���Ƿ��Ѿ�װ�����װ����ͬ���͵�װ��
        {
            if(item.Key.equipmentType==newEquipment.equipmentType)
            {
                oldEquipment = item.Key;
            }
        }

        if(oldEquipment!=null)//�Ѿ�װ�����滻
        {
            UnequiItem(oldEquipment);
            AddItem(oldEquipment);//��ԭװ���Żزֿ�
        }

        equipment.Add(newItem);//װ����װ��
        equipmentDictionary.Add(newEquipment, newItem);
        newEquipment.AddModifiers();

        RemoveItem(_item);//�Ӳֿ����Ƴ�

        UpdateSlotUI();
    }

    public void UnequiItem(ItemData_Equipment itemToRemove)
    {
        if (equipmentDictionary.TryGetValue(itemToRemove, out InventoryItem value))
        {
            equipment.Remove(value);
            equipmentDictionary.Remove(itemToRemove);
            itemToRemove.RemoveModifiers();
        }
    }


    private void UpdateSlotUI()
    {
        for (int i = 0; i < equipmentSlot.Length; i++)
        {
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)//����װ����UI
            {
                if (item.Key.equipmentType == equipmentSlot[i].slotType)
                {
                    equipmentSlot[i].UpdateSlot(item.Value);
                }
            }
        }
        for (int i = 0; i < inventoryItemSlot.Length; i++)
        {
            inventoryItemSlot[i].CleanUpSlot();
        }
        for (int i = 0; i < stashItemSlot.Length; i++)
        {
            stashItemSlot[i].CleanUpSlot();
        }



        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryItemSlot[i].UpdateSlot(inventory[i]);
        }

        for (int i = 0; i < stash.Count; i++)
        {
            stashItemSlot[i].UpdateSlot(stash[i]);
        }

        UpdateStasUI();
    }

    public void UpdateStasUI()
    {
        for (int i = 0; i < statSlot.Length; i++)//����״̬��Ϣ
        {
            statSlot[i].UpdateStatValueUI();
        }
    }

    public void AddItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equipment && CanAdditem())//�����ӵ���װ��
        {
            AddToInventory(_item);//��ӵ�װ�����
        }
        else if(_item.itemType == ItemType.Material)
        {
            AddToStash(_item);
        }



        UpdateSlotUI();
    }


    private void AddToInventory(ItemData _item)//���װ��
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventory.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
    }

    private void AddToStash(ItemData _item)//��Ӳ���
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }
    }

    public void RemoveItem(ItemData _item) 
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))//�Ƴ�װ��
        {
            if (value.stackSize <= 1)
            {
                inventory.Remove(value);
                inventoryDictionary.Remove(_item);
            }
            else
            {
                value.RemoveStack();
            }
        }
        else if(stashDictionary.TryGetValue(_item,out InventoryItem stashValue))//�Ƴ�����
        { 
            if(stashValue.stackSize<=1)
            {
                stash.Remove(stashValue);
                stashDictionary.Remove(_item);
            }
            else
            {
                stashValue.RemoveStack();
            }
        }

        UpdateSlotUI();
    }

    public bool CanAdditem()
    {
        if(inventory.Count>=inventoryItemSlot.Length)
        {
            return false;
        }
        return true;
    }

    public bool CanCraft(ItemData_Equipment _itemToCraft, List<InventoryItem> _requiredMaterials)
    {
        List<InventoryItem> materialsToRemove = new List<InventoryItem>();

        for (int i = 0; i < _requiredMaterials.Count; i++)
        {
            if (stashDictionary.TryGetValue(_requiredMaterials[i].data, out InventoryItem stashValue))
            {
                if (stashValue.stackSize < _requiredMaterials[i].stackSize)
                {
                    Debug.Log("not enough materials");
                    return false;
                }
                else
                {
                    materialsToRemove.Add(stashValue);
                }

            }
            else
            {
                Debug.Log("not enough materials");
                return false;
            }
        }


        for (int i = 0; i < materialsToRemove.Count; i++)
        {
            RemoveItem(materialsToRemove[i].data);
        }

        AddItem(_itemToCraft);
        Debug.Log("Here is your item " + _itemToCraft.name);

        return true;
    }
    public List<InventoryItem> GetEquipmentList() => equipment;

    public List<InventoryItem> GetStashList() => stash;

    public ItemData_Equipment GetEquipment(EquipmentType _type)//�õ���ǰtype���͵�װ��
    {
        ItemData_Equipment equipedItem = null;

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == _type)
            {
                equipedItem = item.Key;
            }
        }

        return equipedItem;
    }

    public bool UseFlask()
    {
        ItemData_Equipment currentFlask=GetEquipment(EquipmentType.Flask);

        if (currentFlask==null)
        {
            return false;
        }
        bool canUseFlask = (Time.time > lastTimeUsedFlask + flaskCooldown);

        if(canUseFlask)
        {
            flaskCooldown = currentFlask.itemCooldown;
            currentFlask.Effect(null);
            lastTimeUsedFlask = Time.time;
            return true;
        }
        return false;
    }

    public bool CanUseArmor()
    {
        ItemData_Equipment currentArmor=GetEquipment(EquipmentType.Armor);

        if(Time.time > lastTimeUsedArmor+armorCooldown)
        {
            armorCooldown = currentArmor.itemCooldown;
            lastTimeUsedArmor = Time.time;
            return true;
        }

        return false;
    }

    public void LoadData(GameData _data)
    {
        loadedItems.Clear();
        loadedEquipment.Clear();

        foreach(KeyValuePair<string,int> pair in _data.inventory)
        {
            foreach(var item in itemDataBase)
            {
                if(item !=null && item.itemId== pair.Key)
                {
                    InventoryItem itemToLoad = new InventoryItem(item);
                    itemToLoad.stackSize=pair.Value;

                    loadedItems.Add(itemToLoad);
                }
            }
        }

        foreach(string loadedItemId in _data.equipmentId)
        {
            foreach(var item in itemDataBase)
            {
                if(item!=null && loadedItemId==item.itemId)
                {
                    loadedEquipment.Add(item as ItemData_Equipment);
                }
            }
        }

    }

    public void SaveData(ref GameData _data)
    {
        _data.inventory.Clear();
        _data.equipmentId.Clear();

        foreach(KeyValuePair<ItemData,InventoryItem> pair in inventoryDictionary)
        {
            _data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
        }

        foreach(KeyValuePair<ItemData,InventoryItem> pair in stashDictionary)
        {
            _data.inventory.Add(pair.Key.itemId,pair.Value.stackSize);
        }

        foreach(KeyValuePair<ItemData_Equipment,InventoryItem> pair in equipmentDictionary)
        {
            _data.equipmentId.Add(pair.Key.itemId);
        }
    }

    public void ReLoadItem()
    {
        List<ItemData_Equipment> toInitEquipment = new List<ItemData_Equipment>();
        List<ItemData> toInitInventory = new List<ItemData>();
        List<ItemData> toInitStash = new List<ItemData>();
        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            toInitEquipment.Add(item.Key);
        }
        foreach (var item in toInitEquipment)
        {
            UnequiItem(item);
        }
        foreach (KeyValuePair<ItemData, InventoryItem> item in inventoryDictionary)
        {
            toInitInventory.Add(item.Key);
        }
        foreach (KeyValuePair<ItemData, InventoryItem> item in stashDictionary)
        {
            toInitStash.Add(item.Key);
        }
        foreach (var item in toInitInventory)
        {
            RemoveItem(item);
        }
        foreach (var item in toInitStash)
        {
            RemoveItem(item);
        }

        for (int i = 0; i < equipmentSlot.Length; i++)
        {
            equipmentSlot[i].CleanUpSlot();
        }
        Debug.Log("Init inventory");

    }

#if UNITY_EDITOR
    [ContextMenu("Fill up item data base")]
    private void FillUpItemDataBase()=>itemDataBase=new List<ItemData>(GetItemsDataBase());
    private List<ItemData> GetItemsDataBase()
    {
        List<ItemData> itemDataBase=new List<ItemData>();
        string[] assetName = AssetDatabase.FindAssets("", new[] { "Assets/Data/Items" });

        foreach(string SOName in assetName)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOName);
            var itemData=AssetDatabase.LoadAssetAtPath<ItemData>(SOpath);
            itemDataBase.Add(itemData);
        }

        return itemDataBase;
    }
#endif

}
