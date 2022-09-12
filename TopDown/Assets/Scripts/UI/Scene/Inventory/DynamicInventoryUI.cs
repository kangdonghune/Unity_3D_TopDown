using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicInventoryUI : InventoryUI
{
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

    public override void CreateSlotUIs()
    {
        slotUIs = new Dictionary<GameObject, InventorySlot>();

        for (int i = 0; i < inventoryObject.slots.Length; i ++)
        {
            GameObject go = Managers.Resource.Instantiate(slotPrefab, Vector2.zero, Quaternion.identity, transform);
            go.GetComponent<RectTransform>().anchoredPosition = CalculatePosition(i);

            AddEvent(go, EventTriggerType.PointerEnter, delegate { OnEnterSlot(gameObject); });
            AddEvent(go, EventTriggerType.PointerExit, delegate { OnExitSlot(gameObject); });
            AddEvent(go, EventTriggerType.BeginDrag, delegate { OnStartDrag(gameObject); });
            AddEvent(go, EventTriggerType.Drag, delegate { OnDrag(gameObject); });
            AddEvent(go, EventTriggerType.EndDrag, delegate { OnEndDrag(gameObject); });

            inventoryObject.slots[i].slotUI = go;
            slotUIs.Add(go, inventoryObject.slots[i]);

            go.name += ": " + i;

        }
    }

    public Vector3 CalculatePosition(int i)
    {
        float x = start.x + ((space.x + size.x) * (i % numberOfColumn));
        float y = start.y + (-(space.y + size.y) * (i / numberOfColumn));
        return new Vector3(x,y,0f);
    }


}
