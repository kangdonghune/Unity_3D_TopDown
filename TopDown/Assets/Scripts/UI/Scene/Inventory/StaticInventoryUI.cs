using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StaticInventoryUI : InventoryUI
{
    public GameObject[] staticSlots = null;

    protected int numberOfColumn = 3;

    [SerializeField]
    protected Vector2 start;

    [SerializeField]
    protected Vector2 size;

    [SerializeField]
    protected Vector2 space;

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

            inventoryObject.slots[i].slotUI = go;
            slotUIs.Add(go, inventoryObject.slots[i]);
        }
    }

    public override void OnRButtonDown(GameObject go)
    {
    }


    public override bool AddItemPossible(ItemObject itemObj, int amount)
    {
        //��� �κ��丮 ������ ��ȸ�ϸ� �ְ��� �ϴ� �������� Ÿ���� ���ĭ�� ��������� �ش� ĭ�� ���� ���� true ��ȯ.
        foreach(InventorySlot slot in  inventoryObject.slots)
        {
            if(slot.item.id < 0) //�ش� ������ ����ִٸ�
            {
                foreach(ItemType type in slot.allowedItems) //�ش� ���� ���� ���� ������ ��ȸ�ϸ� �ְ��� �ϴ� �����۰� ��
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
