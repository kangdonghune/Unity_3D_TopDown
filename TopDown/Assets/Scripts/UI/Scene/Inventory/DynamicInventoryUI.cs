using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicInventoryUI : InventoryUI
{
    [SerializeField]
    protected ItemObjectDatabase database;

    [SerializeField]
    public StaticInventoryUI equipment;



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
            go.GetComponent<RectTransform>().sizeDelta = size;

            AddEvent(go, EventTriggerType.PointerEnter, delegate { OnEnterSlot(go); });
            AddEvent(go, EventTriggerType.PointerExit, delegate { OnExitSlot(go); });
            AddEvent(go, EventTriggerType.BeginDrag, delegate { OnStartDrag(go); });
            AddEvent(go, EventTriggerType.Drag, delegate { OnDrag(go); });
            AddEvent(go, EventTriggerType.EndDrag, delegate { OnEndDrag(go); });


            inventoryObject.slots[i].slotUI = go;
            inventoryObject.slots[i].parent = inventoryObject;
            slotUIs.Add(go, inventoryObject.slots[i]);

            go.name += ": " + i;
            go.FindChild<TextMeshProUGUI>().text = i.ToString();
           

            slotUIs[go].parent.AddItem(database.itemObjects[0].data,2);
        }
    }

    public override void OnRButtonDown(GameObject go)
    {
        //TODO - 우클릭 시 장비면 플레이어 인벤에 있는 슬롯 중에 교환 가능한 파트와 교체 소모품이면 사용

    }

    public override bool AddItemPossible(ItemObject itemObj, int amount)
    {
        return inventoryObject.AddItem(itemObj.data, amount);
    }

    private void SetEquipment()
    {
        if (MouseData.slotHoveredOver)
        {
            InventorySlot mouseHoverSlotDatas = MouseData.interfaceMouseIsOver.slotUIs[MouseData.slotHoveredOver];
            for(int i = 0; i < equipment.inventoryObject.slots.Length; i++)
            {
                if (inventoryObject.SwapItems(mouseHoverSlotDatas, equipment.inventoryObject.slots[i]))
                    break;
            }
        }

    }
    private void UsingConsumable()
    {
        if (MouseData.slotHoveredOver) 
        {
            InventorySlot mouseHoverSlotDatas = MouseData.interfaceMouseIsOver.slotUIs[MouseData.slotHoveredOver];
            mouseHoverSlotDatas.RemoveAmount();
        }
    }

    public Vector3 CalculatePosition(int i)
    {
        float x = start.x + ((space.x + size.x) * (i % numberOfColumn));
        float y = start.y + (-(space.y + size.y) * (i / numberOfColumn));
        return new Vector3(x, y, 0f);
    }

}
