using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SkillUI : InventoryUI
{
    public GameObject[] staticSlots = null;
    public GameObject TextBoard = null;

    protected override void Awake()
    {
        //인벤토리를 그대로 넣으면 해당 인벤토리 객체가 공유되고 또한 저장되는 현상이 발생. 복사 생성하여 임시 객체로 사용
        InventoryObject original = Resources.Load<InventoryObject>("Prefab/UI/Inventory/SKill");
        inventoryObject = Instantiate(original);

        CreateSlotUIs();

        for (int i = 0; i < inventoryObject.slots.Length; i++)
        {
            //전부 new로 만들어질 때 디펄트 값으로 되어 있으니 인벤토리UI가 처음 만들어 질 때 부모를 연결
            inventoryObject.slots[i].parent = inventoryObject;
        }
    }

    public override void CreateSlotUIs()
    {
        slotUIs = new Dictionary<GameObject, InventorySlot>();

        for (int i = 0; i < inventoryObject.slots.Length; i++)
        {
            GameObject go = staticSlots[i];

            AddEvent(go, EventTriggerType.PointerEnter, delegate { OnEnterSlot(go); });
            AddEvent(go, EventTriggerType.PointerExit, delegate { OnExitSlot(go); });

            inventoryObject.slots[i].slotUI = go;
            inventoryObject.slots[i].parent = inventoryObject;
            slotUIs.Add(go, inventoryObject.slots[i]);

        }
    }

    private void Update()
    {
        
    }

    public override void OnEnterSlot(GameObject go)
    {
        base.OnEnterSlot(go);
        TextBoard.SetActive(true);
        string test = MouseData.slotHoveredOver.GetComponent<SkillText>().Text;
        TextBoard.GetComponentInChildren<TextMeshProUGUI>().text = MouseData.slotHoveredOver.GetComponent<SkillText>().Text;

    }

    public override void OnExitSlot(GameObject go)
    {
        base.OnExitSlot(go);
        TextBoard.SetActive(false);
    }
}
