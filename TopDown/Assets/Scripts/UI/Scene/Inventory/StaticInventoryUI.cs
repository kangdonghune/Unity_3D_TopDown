using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StaticInventoryUI : InventoryUI
{
    public GameObject[] staticSlots = null;

    protected int numberOfColumn = 3;

    private DynamicInventoryUI _inventory;

    [SerializeField]
    protected Vector2 start;

    [SerializeField]
    protected Vector2 size;

    [SerializeField]
    protected Vector2 space;

    protected override void Awake()
    {
        //�κ��丮�� �״�� ������ �ش� �κ��丮 ��ü�� �����ǰ� ���� ����Ǵ� ������ �߻�. ���� �����Ͽ� �ӽ� ��ü�� ���
        InventoryObject original = Resources.Load<InventoryObject>("Prefab/UI/Inventory/PlayerEquipment");
        inventoryObject = Object.Instantiate(original);
        //�κ��丮 ���� �� �Ŵ����� �߰�
        Managers.UI.inventorys.Add(inventoryObject);
        Managers.UI.inventoryUIs.Add(this);
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        //�ν��Ͻ�ȭ �� �κ��丮�� �����;� �ؼ� �ν����͸� ���� ������ �����ؾ��Ѵ�.
        _inventory = gameObject.transform.parent.gameObject.GetComponentInChildren<DynamicInventoryUI>();
    }
    public override void CreateSlotUIs()
    {
        slotUIs = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < inventoryObject.slots.Length; i++)
        {
            GameObject go =  staticSlots[i];
            go.GetComponent<RectTransform>().sizeDelta = size;
 

            AddEvent(go, EventTriggerType.PointerEnter, delegate { OnEnterSlot(go); });
            AddEvent(go, EventTriggerType.PointerExit, delegate { OnExitSlot(go); });
            AddEvent(go, EventTriggerType.BeginDrag, delegate { OnStartDrag(go); });
            AddEvent(go, EventTriggerType.Drag, delegate { OnDrag(go); });
            AddEvent(go, EventTriggerType.EndDrag, delegate { OnEndDrag(go); });
            AddEvent(go, EventTriggerType.PointerClick, (data) => { OnClick(go, (PointerEventData)data); });

            inventoryObject.slots[i].slotUI = go;
            inventoryObject.slots[i].OnPostUpdate += OnPostUpdate;
            slotUIs.Add(go, inventoryObject.slots[i]);
        }
    }

    public override void OnEnterSlot(GameObject go)
    {
        base.OnEnterSlot(go);
        UpdateItemBox();
        if(ItemTextBox != null)
            ItemTextBox.transform.GetChild(0).GetComponent<RectTransform>().pivot = new Vector2(0f, 0f);

    }

    public override void OnExitSlot(GameObject go)
    {
        base.OnExitSlot(go);
        Managers.Resource.Destroy(ItemTextBox);
    }

    //public override void OnLButtonClick(InventorySlot slot)
    //{

    //}

    public override void OnRButtonClick(InventorySlot slot)
    {
        if (_inventory == null)
            return;
        InventorySlot emptySlot = _inventory.inventoryObject.GetEmptySlot();
        if (emptySlot != null)
        {
            emptySlot.parent.AddItem(slot.item, slot.amount); //�� ���Կ� ������ �߰�
            slot.RemoveItem();// ������ ����
        }
        Managers.Sound.Play("ItemGet");
        Managers.Resource.Destroy(ItemTextBox);
    }

    public override bool AddItemPossible(ItemObject itemObj, int amount)
    {
        //��� �κ��丮 ������ ��ȸ�ϸ� �ְ��� �ϴ� �������� Ÿ���� ���ĭ�� ��������� �ش� ĭ�� ���� ���� true ��ȯ.
        foreach(InventorySlot slot in  inventoryObject.slots)
        {
            if(slot.item.id < 0) //�ش� ������ ����ִٸ�
            {
                foreach(Define.ItemType type in slot.allowedItems) //�ش� ���� ���� ���� ������ ��ȸ�ϸ� �ְ��� �ϴ� �����۰� ��
                {
                    if (itemObj.type == type)
                    {
                        slot.UpdateSlot(itemObj.data, amount);
                        return true;
                    }

                }
            }
        }
        return false;
    }
}
