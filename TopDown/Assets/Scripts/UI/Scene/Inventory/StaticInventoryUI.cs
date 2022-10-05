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
        //인벤토리를 그대로 넣으면 해당 인벤토리 객체가 공유되고 또한 저장되는 현상이 발생. 복사 생성하여 임시 객체로 사용
        InventoryObject original = Resources.Load<InventoryObject>("Prefab/UI/Inventory/PlayerEquipment");
        inventoryObject = Object.Instantiate(original);
        //인벤토리 생성 시 매니저에 추가
        Managers.UI.inventorys.Add(inventoryObject);
        Managers.UI.inventoryUIs.Add(this);
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        //인스턴스화 된 인벤토리를 가져와야 해서 인스펙터를 통한 연결을 방지해야한다.
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
            emptySlot.parent.AddItem(slot.item, slot.amount); //빈 슬롯에 아이템 추가
            slot.RemoveItem();// 아이템 제거
        }
        Managers.Sound.Play("ItemGet");
        Managers.Resource.Destroy(ItemTextBox);
    }

    public override bool AddItemPossible(ItemObject itemObj, int amount)
    {
        //장비 인벤토리 슬롯을 순회하며 넣고자 하는 아이템의 타입의 장비칸이 비어있으면 해당 칸에 장착 이후 true 반환.
        foreach(InventorySlot slot in  inventoryObject.slots)
        {
            if(slot.item.id < 0) //해당 슬롯이 비어있다면
            {
                foreach(Define.ItemType type in slot.allowedItems) //해당 슬롯 장착 가능 종류를 순회하며 넣고자 하는 아이템과 비교
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
