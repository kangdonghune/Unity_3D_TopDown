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
        //장비 인벤토리 슬롯을 순회하며 넣고자 하는 아이템의 타입의 장비칸이 비어있으면 해당 칸에 장착 이후 true 반환.
        foreach(InventorySlot slot in  inventoryObject.slots)
        {
            if(slot.item.id < 0) //해당 슬롯이 비어있다면
            {
                foreach(ItemType type in slot.allowedItems) //해당 슬롯 장착 가능 종류를 순회하며 넣고자 하는 아이템과 비교
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
