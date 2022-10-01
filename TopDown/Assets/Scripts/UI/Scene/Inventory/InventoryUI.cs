using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class MouseData
{
    public static InventoryUI interfaceMouseIsOver; //현재 마우스가 위치한 인벤토리 UI
    public static GameObject slotHoveredOver; //현재 마우스가 위치한 슬롯
    public static GameObject preSlotHoveredOver; //마지막으로 마우스가 위치한 슬롯
    public static GameObject  tempItemBeginDragged; //현재 마우스가 드래그 중인 임시 이미지
    
}

[RequireComponent(typeof(EventTrigger))]
public abstract class InventoryUI : MonoBehaviour
{
    [HideInInspector]
    public InventoryObject inventoryObject;
    public GameObject Owner { get; set; } = null;
    private InventoryObject preInventoryObject;

    public Dictionary<GameObject, InventorySlot> slotUIs = new Dictionary<GameObject, InventorySlot>();
    #region UnityFunc
    protected virtual void Awake()
    {
        CreateSlotUIs();

        for (int i = 0; i < inventoryObject.slots.Length; i++)
        {
            //전부 new로 만들어질 때 디펄트 값으로 되어 있으니 인벤토리UI가 처음 만들어 질 때 부모를 연결
            inventoryObject.slots[i].parent = inventoryObject;
        }

        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });
    }

    protected virtual void Start()
    {
        for(int i = 0; i < inventoryObject.slots.Length; i++)
        {
            //인벤토리오브젝트에 정보가 있다면 해당 정보로 슬롯을 갱신
            inventoryObject.slots[i].UpdateSlot(inventoryObject.slots[i].item, inventoryObject.slots[i].amount);
        }
    }
    #endregion

    public abstract void CreateSlotUIs();

    #region Event
    protected void AddEvent(GameObject go, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = go.GetOrAddComponent<EventTrigger>();
        EventTrigger.Entry eventTrigger = new EventTrigger.Entry() { eventID = type };
        eventTrigger.callback.RemoveListener(action);//이미 있는 상태라면 제거를 해줘야 중복 추가가 안된다.
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }
    
    //아이템에 대한 아이콘과 개수를 표시
    public void OnPostUpdate(InventorySlot slot)
    {
        if (slot == null)
            return;
        slot.slotUI.transform.GetChild(0).GetComponent<Image>().sprite = slot.item.id < 0 ? null : slot.ItemObject.icon;
        slot.slotUI.transform.GetChild(0).GetComponent<Image>().color = slot.item.id < 0 ? new Color(1, 1, 1, 0) : new Color(1, 1, 1, 1);
        slot.slotUI.FindChild<TextMeshProUGUI>().text = slot.item.id < 0 ? string.Empty : (slot.amount == 1 ? string.Empty : slot.amount.ToString("n0"));
    }

    public void OnEnterInterface(GameObject go)
    {
        MouseData.interfaceMouseIsOver = go.GetComponent<InventoryUI>();
    }

    public void OnExitInterface(GameObject go)
    {
        MouseData.interfaceMouseIsOver = null;
    }

    public virtual void OnEnterSlot(GameObject go)
    {
        MouseData.slotHoveredOver = go;
        MouseData.preSlotHoveredOver = go;
    }

    public virtual void OnExitSlot(GameObject go)
    {
        MouseData.slotHoveredOver = null;
    }

    public void OnStartDrag(GameObject go)
    {
        MouseData.tempItemBeginDragged = CreateDragImage(go);
    }

    public void OnDrag(GameObject go)
    {
        if (MouseData.tempItemBeginDragged == null)
            return;
        MouseData.tempItemBeginDragged.GetComponent<RectTransform>().position = Input.mousePosition;
    }

    public void OnEndDrag(GameObject go)
    {
        Destroy(MouseData.tempItemBeginDragged);
        if(MouseData.interfaceMouseIsOver == null) // 인벤토리 밖으로 드래그 했다면 인벤토리에서 아이템 제거
        {
            if (slotUIs[go].ItemObject == null)
                return;
            GameObject groundItme = Managers.Resource.Instantiate("UI/Inventory/GroundItem");//바닥 아이템 생성
            groundItme.transform.position = Owner.transform.position;
            groundItme.GetComponent<GroundItem>().ItemObject = slotUIs[go].ItemObject;//아이템복사
            groundItme.GetComponent<GroundItem>().ItemObject.itemAmount = slotUIs[go].amount; //아이템 개수 저장
            slotUIs[go].RemoveItem();//아이템 제거
        }
        else if(MouseData.slotHoveredOver) //드래그 종료 지점에 슬롯이 존재한다면 해당 슬롯의 아이템과 드래그하고 있던 것을 교체
        {
            InventorySlot mouseHoverSlotDatas = MouseData.interfaceMouseIsOver.slotUIs[MouseData.slotHoveredOver];
            inventoryObject.SwapItems(slotUIs[go], mouseHoverSlotDatas);
        }

    }


    public void OnClick(GameObject go, PointerEventData data)
    {
        InventorySlot slot = slotUIs[go];
        if(slot == null)
        {
            return;
        }
        //인벤토리에 장비 착용 등은 우클릭만 이용할 예정. 추후 필요시 활성화
        //if(data.button == PointerEventData.InputButton.Left)
        //{
        //    OnLButtonClick(slot);
        //}
        if (data.button == PointerEventData.InputButton.Right)
        {
            OnRButtonClick(slot);
        }

    }

    public virtual void OnLButtonClick(InventorySlot slot)
    {

    }

    public virtual void OnRButtonClick(InventorySlot slot)
    {

    }

    private GameObject CreateDragImage(GameObject go)
    {
        if(slotUIs[go]?.item.id < 0)
        {
            return null;
        }

        GameObject dragImage = new GameObject();
        RectTransform rectTransform = dragImage.AddComponent<RectTransform>();
        rectTransform.sizeDelta = go.GetComponent<RectTransform>().sizeDelta;
        dragImage.transform.SetParent(transform.parent); //canverse
        Image image = dragImage.AddComponent<Image>();
        image.sprite = slotUIs[go].ItemObject.icon;
        image.raycastTarget = false; // 유저입력 방지
        dragImage.name = "Drag Image";

        return dragImage;
    }

    #endregion

    public virtual bool AddItemPossible(ItemObject item, int amount) { return false; }
    
}


