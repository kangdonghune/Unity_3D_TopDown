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
            //����ƽ �κ��丮�� ������ �κ��丮 ������Ʈ(Scriptable)�� ����Ʈ�� �ִ� ������ ����� ä�� ������ �ȴ�.
            //���� ����� �� �� ���·� �����ϱ� ���ؼ� ���� �� �ش� ����Ʈ�� ���� ����� �ʿ䰡 �ִ�.
            slotUIs[staticSlots[i]].RemoveItem();
        }
    }
}
