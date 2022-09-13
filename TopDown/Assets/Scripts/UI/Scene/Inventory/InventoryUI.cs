using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class MouseData
{
    public static InventoryUI interfaceMouseIsOver; //���� ���콺�� ��ġ�� �κ��丮 UI
    public static GameObject slotHoveredOver; //���� ���콺�� ��ġ�� ����
    public static GameObject  tempItemBeginDragged; //���� ���콺�� �巡�� ���� �ӽ� �̹���
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
            //���� new�� ������� �� ����Ʈ ������ �Ǿ� ������ �κ��丮UI�� ó�� ����� �� �� �θ� ����
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
            //�κ��丮������Ʈ�� ������ �ִٸ� �ش� ������ ������ ����
            inventoryObject.slots[i].UpdateSlot(inventoryObject.slots[i].item, inventoryObject.slots[i].amount);
        }
    }


    public abstract void CreateSlotUIs();

    protected void AddEvent(GameObject go, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = go.GetOrAddComponent<EventTrigger>();
        EventTrigger.Entry eventTrigger = new EventTrigger.Entry() { eventID = type };
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }
    
    //�����ۿ� ���� �����ܰ� ������ ǥ��
    public void OnPostUpdate(InventorySlot slot)
    {
        slot.slotUI.transform.GetChild(0).GetComponent<Image>().sprite = slot.item.id < 0 ? null : slot.ItemObject.icon;
        slot.slotUI.transform.GetChild(0).GetComponent<Image>().color = slot.item.id < 0 ? new Color(1, 1, 1, 0) : new Color(1, 1, 1, 1);
        //slot.slotUI.FindChild<TextMeshProUGUI>().text = slot.item.id < 0 ? string.Empty : (slot.amount == 1 ? string.Empty : slot.amount.ToString("n0"));
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
        if(MouseData.interfaceMouseIsOver == null) // �κ��丮 ������ �巡�� �ߴٸ� �κ��丮���� ������ ����
        {
            slotUIs[go].RemoveItem();
        }
        else if(MouseData.slotHoveredOver) //�巡�� ���� ������ ������ �����Ѵٸ� �ش� ������ �����۰� �巡���ϰ� �ִ� ���� ��ü
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
        image.raycastTarget = false; // �����Է� ����
        dragImage.name = "Drag Image";

        return dragImage;
    }

}

