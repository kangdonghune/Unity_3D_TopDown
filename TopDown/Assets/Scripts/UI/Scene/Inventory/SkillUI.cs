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
        //인벤토리를 그대로 넣으면 해당 인벤토리 객체가 공유되고 또한 저장되는 현상이 발생. 복사 생성하여 임시 객체로 사용
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
        //필터 1 쿨타임텍스트 2
        //1.스킬이 비활성화 상태면 필터 on , 쿨타임 x
        AttackBehavior behavior = go.GetComponent<SkillInfo>().behavior;
        if(behavior.Active == false)
        {
            go.transform.GetChild(1).GetComponent<Image>().enabled = true;
            go.transform.GetChild(2).GetComponent<TextMeshProUGUI>().enabled = false;
            return;
        }
        //2.스킬이 활성화 상태고 쿨타임이 없다면 필터x, 
        if (behavior.isAvailable)
        {
            go.transform.GetChild(1).GetComponent<Image>().enabled = false;
            go.transform.GetChild(2).GetComponent<TextMeshProUGUI>().enabled = false;
            return;
        }
        //3.스킬이 활성화 상태인데 쿨타임이 있다면 필터 on, 쿨타임 on
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
