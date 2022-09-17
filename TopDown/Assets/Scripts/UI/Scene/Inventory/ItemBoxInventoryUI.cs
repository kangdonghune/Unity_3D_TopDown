using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemBoxInventoryUI : InventoryUI
{
    [SerializeField]
    protected ItemObjectDatabase database;

    public GameObject ConnectUserInven = null;
    private StaticInventoryUI equipment;
    private DynamicInventoryUI inven;



    [SerializeField]
    protected GameObject slotPrefab;

    [SerializeField]
    protected Vector2 start;

    [SerializeField]
    protected Vector2 size;

    [SerializeField]
    protected Vector2 space;

    [Min(1), SerializeField]
    protected int numberOfColumn = 5;

    protected override void Awake()
    {
        //�κ��丮�� �״�� ������ �ش� �κ��丮 ��ü�� �����ǰ� ���� ����Ǵ� ������ �߻�. ���� �����Ͽ� �ӽ� ��ü�� ���
        InventoryObject original = Resources.Load<InventoryObject>("Prefab/UI/Inventory/ItemBoxInventory");
        inventoryObject = Object.Instantiate(original);
        base.Awake();
    }


    public override void CreateSlotUIs()
    {
        slotUIs = new Dictionary<GameObject, InventorySlot>();

        for (int i = 0; i < inventoryObject.slots.Length; i++)
        {
            GameObject go = Managers.Resource.Instantiate(slotPrefab, Vector2.zero, Quaternion.identity, transform);
            go.GetComponent<RectTransform>().anchoredPosition = CalculatePosition(i);
            go.GetComponent<RectTransform>().sizeDelta = size;

            AddEvent(go, EventTriggerType.PointerEnter, delegate { OnEnterSlot(go); });
            AddEvent(go, EventTriggerType.PointerExit, delegate { OnExitSlot(go); });
            AddEvent(go, EventTriggerType.PointerClick, (data) => { OnClick(go, (PointerEventData)data); });


            inventoryObject.slots[i].slotUI = go;
            inventoryObject.slots[i].parent = inventoryObject;
            slotUIs.Add(go, inventoryObject.slots[i]);
            go.name += ": " + i;

            slotUIs[go].parent.AddItem(database.itemObjects[1].data, 10);
        }
    }

    public Vector3 CalculatePosition(int i)
    {
        float x = start.x + ((space.x + size.x) * (i % numberOfColumn));
        float y = start.y + (-(space.y + size.y) * (i / numberOfColumn));
        return new Vector3(x, y, 0f);
    }

    public override void OnRButtonClick(InventorySlot slot)
    {
        //slot�� �������� ���ų� ����� ���� �κ��� ���� ��Ȳ�̶��
        if (slot.ItemObject == null || ConnectUserInven == null)
            return;
        SendItem(slot);
    }

    private void SendItem(InventorySlot ItemBoxslot)
    {
        for (int i = 0; i < equipment.inventoryObject.slots.Length; i++)
        {
            if (equipment.inventoryObject.slots[i].ItemObject == null)
            {
                if (inventoryObject.SwapItems(ItemBoxslot, equipment.inventoryObject.slots[i]))
                    return;
            }
              
        }
        inven.inventoryObject.AddItem(ItemBoxslot.item, ItemBoxslot.amount);
        ItemBoxslot.RemoveItem();
    }

    public bool ConnectInven(GameObject invenGo)
    {
        if (invenGo == null)
            return false;
        ConnectUserInven = invenGo;
        equipment = invenGo.FindChild<StaticInventoryUI>();
        inven = invenGo.FindChild<DynamicInventoryUI>();

        if (equipment == null || inven == null)
            return false;

        return true;
    }

    public void DisConnectInven()
    {
        ConnectUserInven = null;
        equipment = null;
        inven = null;
    }
}
