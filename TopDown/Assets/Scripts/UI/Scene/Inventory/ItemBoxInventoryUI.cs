using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemBoxInventoryUI : InventoryUI
{
    public GameObject ConnectUserInven = null;
    private StaticInventoryUI equipment;
    private DynamicInventoryUI inven;
    public ItemBoxDatabase database;
    private ItemBoxContents Contents;



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
        //인벤토리를 그대로 넣으면 해당 인벤토리 객체가 공유되고 또한 저장되는 현상이 발생. 복사 생성하여 임시 객체로 사용
        InventoryObject original = Resources.Load<InventoryObject>("Prefab/UI/Inventory/ItemBoxInventory");
        inventoryObject = Instantiate(original);
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
        }

        foreach (ItemBoxContents contents in database.Contents)
        {
            string tag = transform.parent.parent.tag;
            if (tag == Enum.GetName(typeof(Define.Tag), contents.tag))
            {
                Contents = contents;
                break;
            }
        }

        for (int i = 0; i < Contents.items.Length; i++)
        {
            if (Contents.items[i].item == null)
                continue;
            inventoryObject.AddItem(Contents.items[i].item.data, Contents.items[i].count);
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
        //slot의 아이템이 없거나 연결된 유저 인벤이 없는 상황이라면
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
        if(inven.inventoryObject.AddItem(ItemBoxslot.item, ItemBoxslot.amount))
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
