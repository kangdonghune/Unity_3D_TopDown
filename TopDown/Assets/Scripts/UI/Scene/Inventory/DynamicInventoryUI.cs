using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicInventoryUI : InventoryUI
{
    [SerializeField]
    protected ItemObjectDatabase database;

    [HideInInspector]
    public StaticInventoryUI equipment;

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
        InventoryObject original = Resources.Load<InventoryObject>("Prefab/UI/Inventory/PlayerInventory");
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
        equipment = gameObject.transform.parent.gameObject.GetComponentInChildren<StaticInventoryUI>();
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
            AddEvent(go, EventTriggerType.BeginDrag, delegate { OnStartDrag(go); });
            AddEvent(go, EventTriggerType.Drag, delegate { OnDrag(go); });
            AddEvent(go, EventTriggerType.EndDrag, delegate { OnEndDrag(go); });
            AddEvent(go, EventTriggerType.PointerClick, (data) => { OnClick(go, (PointerEventData)data); });


            inventoryObject.slots[i].slotUI = go;
            inventoryObject.slots[i].parent = inventoryObject;
            inventoryObject.slots[i].OnPostUpdate += OnPostUpdate;
            slotUIs.Add(go, inventoryObject.slots[i]);

            go.name += ": " + i;
            go.FindChild<TextMeshProUGUI>().text = i.ToString();

        }
    }


    public override void OnEnterSlot(GameObject go)
    {
        base.OnEnterSlot(go);
        UpdateItemBox();
        if (ItemTextBox != null)
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
        if (slot.ItemObject == null) // database에 없는 아이템. 미생성 또는 오류 아이템일 경우 생략
            return;

        if (slot.ItemObject.type < Define.ItemType.Consumable)
            SetEquipment(slot);
        else
            UsingConsumable(slot);
        Managers.Resource.Destroy(ItemTextBox);
        UpdateItemBox();
    }

    public override bool AddItemPossible(ItemObject itemObj, int amount)
    {
        return inventoryObject.AddItem(itemObj.data, amount);
    }

    private void SetEquipment(InventorySlot slot)
    {
        for(int i = 0; i < equipment.inventoryObject.slots.Length; i++)
        {
            //장비칸을 순회하며 교체 가능 시 교체 이후 for문 탈출
            if (inventoryObject.SwapItems(slot, equipment.inventoryObject.slots[i]))
                break;
        }

    }
    private void UsingConsumable(InventorySlot slot)
    {
        slot.parent.OnUseItem(slot.ItemObject);
        slot.RemoveAmount(1);
        Managers.Craft.UpdateContainItems();
    }

    public Vector3 CalculatePosition(int i)
    {
        float x = start.x + ((space.x + size.x) * (i % numberOfColumn));
        float y = start.y + (-(space.y + size.y) * (i / numberOfColumn));
        return new Vector3(x, y, 0f);
    }

}
