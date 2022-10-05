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
        //�κ��丮�� �״�� ������ �ش� �κ��丮 ��ü�� �����ǰ� ���� ����Ǵ� ������ �߻�. ���� �����Ͽ� �ӽ� ��ü�� ���
        InventoryObject original = Resources.Load<InventoryObject>("Prefab/UI/Inventory/ReadyProductInventory");
        inventoryObject = Object.Instantiate(original);
        //�κ��丮 ���� �� �Ŵ����� �߰�
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
            _controller.Stats.AddExp(slot.ItemObject.exp); //���� ����ġ �߰�
            slot.RemoveItem();
        }
        Managers.Craft.UpdateContainItems();//�� ����� �������۵� �����ߴٸ� �ٽ� ���� ������ ������Ʈ
        _controller.StateMachine.ChangeState<PlayerIdleState>();
    }
}
