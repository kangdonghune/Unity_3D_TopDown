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
        //�κ��丮�� �״�� ������ �ش� �κ��丮 ��ü�� �����ǰ� ���� ����Ǵ� ������ �߻�. ���� �����Ͽ� �ӽ� ��ü�� ���
        InventoryObject original = Resources.Load<InventoryObject>("Prefab/UI/Inventory/SKill");
        inventoryObject = Instantiate(original);

        CreateSlotUIs();

        for (int i = 0; i < inventoryObject.slots.Length; i++)
        {
            //���� new�� ������� �� ����Ʈ ������ �Ǿ� ������ �κ��丮UI�� ó�� ����� �� �� �θ� ����
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
