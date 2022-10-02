using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class OnMouseSlotData
{
    public static GameObject slotHoveredOver; //현재 마우스가 위치한 슬롯
    public static GameObject preSlotHoveredOver; //마지막으로 마우스가 위치한 슬롯

}

public abstract class SlotUI : MonoBehaviour
{
    [HideInInspector]
    public InventoryObject inventoryObject;
    public GameObject Owner { get; set; } = null;
    private InventoryObject preInventoryObject;
    protected GameObject ItemTextBox;

    public Dictionary<GameObject, InventorySlot> slotUIs = new Dictionary<GameObject, InventorySlot>();
    #region UnityFunc

    protected virtual void Start()
    {
        for (int i = 0; i < inventoryObject.slots.Length; i++)
        {
            //인벤토리오브젝트에 정보가 있다면 해당 정보로 슬롯을 갱신
            inventoryObject.slots[i].UpdateSlot(inventoryObject.slots[i].item, inventoryObject.slots[i].amount);
        }
    }
    #endregion

    public void UpdateItemBox()
    {
        if (slotUIs[OnMouseSlotData.slotHoveredOver].ItemObject != null)
        {
            ItemTextBox = Managers.Resource.Instantiate("UI/Inventory/ItemTextBox", null, 1);
            ItemTextBox.GetComponentInChildren<ItemInfo>().ItemObject = slotUIs[OnMouseSlotData.slotHoveredOver].ItemObject;
        }
        else
            Managers.Resource.Destroy(ItemTextBox);

    }


    #region Event
    protected void AddEvent(GameObject go, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = go.GetOrAddComponent<EventTrigger>();
        EventTrigger.Entry eventTrigger = new EventTrigger.Entry() { eventID = type };
        //eventTrigger.callback.RemoveListener(action);//이미 있는 상태라면 제거를 해줘야 중복 추가가 안된다.
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

  

    public virtual void OnEnterSlot(GameObject go)
    {
        OnMouseSlotData.slotHoveredOver = go;
        OnMouseSlotData.preSlotHoveredOver = go;
    }

    public virtual void OnExitSlot(GameObject go)
    {
        OnMouseSlotData.slotHoveredOver = null;
    }




    public void OnClick(GameObject go, PointerEventData data)
    {
        InventorySlot slot = slotUIs[go];
        if (slot == null)
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



    #endregion
}