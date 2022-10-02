using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class OnMouseSlotData
{
    public static GameObject slotHoveredOver; //���� ���콺�� ��ġ�� ����
    public static GameObject preSlotHoveredOver; //���������� ���콺�� ��ġ�� ����

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
            //�κ��丮������Ʈ�� ������ �ִٸ� �ش� ������ ������ ����
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
        //eventTrigger.callback.RemoveListener(action);//�̹� �ִ� ���¶�� ���Ÿ� ����� �ߺ� �߰��� �ȵȴ�.
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    //�����ۿ� ���� �����ܰ� ������ ǥ��
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
        //�κ��丮�� ��� ���� ���� ��Ŭ���� �̿��� ����. ���� �ʿ�� Ȱ��ȭ
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