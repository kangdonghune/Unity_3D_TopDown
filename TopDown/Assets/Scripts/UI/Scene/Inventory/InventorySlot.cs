using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventorySlot
{
    public ItemType[] allowedItems = new ItemType[0];

    [NonSerialized]
    public InventoryObject parent;
    [NonSerialized]
    public GameObject slotUI;//슬롯을 표시하는 ui 오브젝트
    [NonSerialized]
    public Action<InventorySlot> OnPreUpdate;
    [NonSerialized]
    public Action<InventorySlot> OnPostUpdate;

    public Item item; //현재 업데이트 된 아이템 종류
    public int amount;//아이템 개수

    //실제 아이템오브젝트
    public ItemObject ItemObject { get { return item.id >= 0 ? parent.database.itemObjects[item.id] : null; } }
    public InventorySlot() => UpdateSlot(new Item(), 0);
    public InventorySlot(Item item, int amount) => UpdateSlot(item, amount);

    public void RemoveItem() => UpdateSlot(new Item(), 0);
    public void RemoveAmount(int value = 1) => AddAmount(-value);

    public void AddAmount(int value = 1)
    {
        if (amount + value <= 0)
            RemoveItem();
        else
            UpdateSlot(item, amount += value);
    }

    public void UpdateSlot(Item item, int amount)
    {
        OnPreUpdate?.Invoke(this);

        this.item = item; //아이템 종류(ex: 갑옷 등)
        this.amount = amount;//아이템 stackable이 true인 경우 2 이상도 가능

        OnPostUpdate?.Invoke(this);
    }

    public bool CanPlaceInSlot(ItemObject itemObject)
    {
        //1.슬롯에 아이템 장착 타입에 제한이 없거나(장비칸X)
        //2.현재 넣고자 하는 아이템이 없거나 업데이트 된 아이템의 타입이 생성되지 않은 경우(빈 인벤토리슬롯) = 슬롯을 비우는 경우
        if (allowedItems.Length <= 0 || itemObject == null || itemObject.data.id < 0)
            return true; //새로운 아이템을 착용할 수 있다.

        foreach (ItemType type in allowedItems)
        {
            if (itemObject.type == type) //슬롯 타입이 맞다면
                return true;
        }

        return false;

    }
}


