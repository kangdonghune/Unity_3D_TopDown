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


    public void OnDestroy()
    {
        for (int i = 0; i < staticSlots.Length; i++)
        {
            //스테틱 인벤토리의 별도의 인벤토리 오브젝트(Scriptable)에 리스트로 있는 슬롯이 변경된 채로 저장이 된다.
            //따라서 재시작 시 빈 상태로 시작하기 위해선 종료 시 해당 리스트를 전부 비워줄 필요가 있다.
            slotUIs[staticSlots[i]].RemoveItem();
        }
    }
}
