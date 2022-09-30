using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillUI : InventoryUI
{
    public GameObject[] SkillsSlots = null;
    public GameObject TextBoard = null;
    public List<AttackBehavior> skills;

    protected override void Awake()
    {
        //�κ��丮�� �״�� ������ �ش� �κ��丮 ��ü�� �����ǰ� ���� ����Ǵ� ������ �߻�. ���� �����Ͽ� �ӽ� ��ü�� ���
        InventoryObject original = Resources.Load<InventoryObject>("Prefab/UI/Inventory/SKill");
        inventoryObject = Instantiate(original);
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        foreach (AttackBehavior behavior in skills)
        {
            foreach(GameObject slot in SkillsSlots)
            {
                if ((int)(slot.name[0]) + 32 == (int)behavior.Key)
                    slot.GetComponent<SkillInfo>().behavior = behavior;
            }
        }
    }

    public override void CreateSlotUIs()
    {
        slotUIs = new Dictionary<GameObject, InventorySlot>();

        for (int i = 0; i < inventoryObject.slots.Length; i++)
        {
            GameObject go = SkillsSlots[i];

            AddEvent(go, EventTriggerType.PointerEnter, delegate { OnEnterSlot(go); });
            AddEvent(go, EventTriggerType.PointerExit, delegate { OnExitSlot(go); });

            inventoryObject.slots[i].slotUI = go;
            inventoryObject.slots[i].parent = inventoryObject;
            slotUIs.Add(go, inventoryObject.slots[i]);

        }
    }

    private void Update()
    {
        foreach (GameObject slot in SkillsSlots)
            UpdateSlot(slot);       
    }

    private void UpdateSlot(GameObject go)
    {
        //���� 1 ��Ÿ���ؽ�Ʈ 2
        //1.��ų�� ��Ȱ��ȭ ���¸� ���� on , ��Ÿ�� x
        AttackBehavior behavior = go.GetComponent<SkillInfo>().behavior;
        if(behavior.Active == false)
        {
            go.transform.GetChild(1).GetComponent<Image>().enabled = true;
            go.transform.GetChild(2).GetComponent<TextMeshProUGUI>().enabled = false;
            return;
        }
        //2.��ų�� Ȱ��ȭ ���°� ��Ÿ���� ���ٸ� ����x, 
        if (behavior.isAvailable)
        {
            go.transform.GetChild(1).GetComponent<Image>().enabled = false;
            go.transform.GetChild(2).GetComponent<TextMeshProUGUI>().enabled = false;
            return;
        }
        //3.��ų�� Ȱ��ȭ �����ε� ��Ÿ���� �ִٸ� ���� on, ��Ÿ�� on
        else
        {
            go.transform.GetChild(1).GetComponent<Image>().enabled = true;
            go.transform.GetChild(2).GetComponent<TextMeshProUGUI>().enabled = true;
            go.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = behavior.RemainCoolTime.ToString("n1");
            return;
        }
    }

    public override void OnEnterSlot(GameObject go)
    {
        base.OnEnterSlot(go);
        TextBoard.SetActive(true);
        string test = MouseData.slotHoveredOver.GetComponent<SkillInfo>().Text;
        TextBoard.GetComponentInChildren<TextMeshProUGUI>().text = MouseData.slotHoveredOver.GetComponent<SkillInfo>().Text;

    }

    public override void OnExitSlot(GameObject go)
    {
        base.OnExitSlot(go);
        TextBoard.SetActive(false);
    }
}
