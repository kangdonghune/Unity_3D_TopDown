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
    public static GameObject  tempItemBeginDragged; //현재 마우스가 드래그 중인 임시 이미지
}

[RequireComponent(typeof(EventTrigger))]
public abstract class InventoryUI : MonoBehaviour
{
    public InventoryObject inventoryObject;
    private InventoryObject preInventoryObject;

    public Dictionary<GameObject, InventorySlot> slotUIs = new Dictionary<GameObject, InventorySlot>();

    private void Awake()
    {
        CreateSlotUIs();

        for (int i = 0; i < inventoryObject.slots.Length; i++)
        {
            //전부 new로 만들어질 때 디펄트 값으로 되어 있으니 인벤토리UI가 처음 만들어 질 때 부모를 연결
            inventoryObject.slots[i].parent = inventoryObject;
            inventoryObject.slots[i].OnPostUpdate += OnPostUpdate;
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


    public abstract void CreateSlotUIs();

    protected void AddEvent(GameObject go, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = go.GetOrAddComponent<EventTrigger>();
        EventTrigger.Entry eventTrigger = new EventTrigger.Entry() { eventID = type };
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Remove(eventTrigger);//이미 있는 상태라면 제거를 해줘야 중복 추가가 안된다.
        trigger.triggers.Add(eventTrigger);
    }
    
    //아이템에 대한 아이콘과 개수를 표시
    public void OnPostUpdate(InventorySlot slot)
    {
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

    public void OnEnterSlot(GameObject go)
    {
        MouseData.slotHoveredOver = go;
    }

    public void OnExitSlot(GameObject go)
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
            slotUIs[go].RemoveItem();
        }
        else if(MouseData.slotHoveredOver) //드래그 종료 지점에 슬롯이 존재한다면 해당 슬롯의 아이템과 드래그하고 있던 것을 교체
        {
            InventorySlot mouseHoverSlotDatas = MouseData.interfaceMouseIsOver.slotUIs[MouseData.slotHoveredOver];
            inventoryObject.SwapItems(slotUIs[go], mouseHoverSlotDatas);
        }

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

}


