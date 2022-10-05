using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ReadyProductsUI : SlotUI
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
    private PlayerController _controller;

    protected void Awake()
    {
        //인벤토리를 그대로 넣으면 해당 인벤토리 객체가 공유되고 또한 저장되는 현상이 발생. 복사 생성하여 임시 객체로 사용
        InventoryObject original = Resources.Load<InventoryObject>("Prefab/UI/Inventory/ReadyProductInventory");
        inventoryObject = Object.Instantiate(original);
        //인벤토리 생성 시 매니저에 추가
        CreateSlotUIs();
        Managers.Craft.OnUpdateEvent -= UpdateSlots;
        Managers.Craft.OnUpdateEvent += UpdateSlots;


    }

    protected override void Start()
    {
        base.Start();
        _controller = Owner.GetComponent<PlayerController>();
    }

    public void CreateSlotUIs()
    {
        slotUIs = new Dictionary<GameObject, InventorySlot>();

        for (int i = 0; i < inventoryObject.slots.Length; i++)
        {
            GameObject go = Managers.Resource.Instantiate(slotPrefab, Vector2.zero, Quaternion.identity, transform);
            go.GetComponent<RectTransform>().anchoredPosition = CalculatePosition(i);
            go.GetComponent<RectTransform>().sizeDelta = size;


            AddEvent(go, EventTriggerType.PointerEnter, delegate { OnEnterSlot(go); });
            AddEvent(go, EventTriggerType.PointerExit, delegate { OnExitSlot(go); });
            AddEvent(go, EventTriggerType.PointerClick, (data) => { OnClick(go, (PointerEventData)data); });
            inventoryObject.slots[i].slotUI = go;
            inventoryObject.slots[i].parent = inventoryObject;
            inventoryObject.slots[i].OnPostUpdate += OnPostUpdate;
            slotUIs.Add(go, inventoryObject.slots[i]);
            go.name += ": " + i;
            go.FindChild<TextMeshProUGUI>().text = i.ToString();
            go.SetActive(false);
        }
    }

    public void UpdateSlots()
    {
        foreach (InventorySlot slot in inventoryObject.slots)
        {
            slot.RemoveItem();
        }
            if (Managers.Craft.readyProduct.Count != 0)
        {
            for(int i =0; i < Managers.Craft.readyProduct.Count && i < inventoryObject.slots.Length; i++)
            {
                inventoryObject.AddItem(Managers.Craft.readyProduct[i].data, 1);
            }
        }
        foreach(InventorySlot slot in inventoryObject.slots)
        {
            if (slot.ItemObject != null)
                slot.slotUI.SetActive(true);
            else
            {
                Managers.Resource.Destroy(ItemTextBox);
                slot.slotUI.SetActive(false);
            }
        }
    }


    public override void OnEnterSlot(GameObject go)
    {
        base.OnEnterSlot(go);
        UpdateItemBox();
        if (ItemTextBox != null)
            ItemTextBox.transform.GetChild(0).GetComponent<RectTransform>().pivot = new Vector2(0f, 0f);
    }

    public override void OnExitSlot(GameObject go)
    {
        base.OnExitSlot(go);
        Managers.Resource.Destroy(ItemTextBox);
    }

    //public override void OnLButtonClick(InventorySlot slot)
    //{

    //}

    public override void OnRButtonClick(InventorySlot slot)
    {
        if(_controller.state != Define.PlayerState.Craft)
           StartCoroutine(CoCrafting(slot, 3f));
    }

    public Vector3 CalculatePosition(int i)
    {
        float x = start.x - ((space.x + size.x) * (i % numberOfColumn));
        float y = start.y - (-(space.y + size.y) * (i / numberOfColumn));
        return new Vector3(x, y, 0f);
    }

    IEnumerator CoCrafting(InventorySlot slot , float time)
    {
        _controller.StateMachine.ChangeState<PlayerCraftState>();
        yield return new WaitForSeconds(time);
        if (Managers.Craft.Craft(slot.ItemObject))
        {
            _controller.Stats.AddExp(slot.ItemObject.exp); //제작 경험치 추가
            slot.RemoveItem();
        }
        Managers.Craft.UpdateContainItems();//다 만들고 재료아이템도 제거했다면 다시 보유 아이템 업데이트
        _controller.StateMachine.ChangeState<PlayerIdleState>();
    }
}
