using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum InterfaceType
{
    Inventory,
    Equiqment,
    Box,
}

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventort/New Inventory")]
public class InventoryObject : ScriptableObject
{
    public ItemObjectDatabase database;
    public InterfaceType type; //장비창인지, 인벤창인지, 퀵슬롯인지 지정

    [SerializeField]
    private Inventory container = new Inventory();

    public InventorySlot[] slots => container.slots;

    public Action<ItemObject> OnUseItem;

    public int EmptySlotCount
    {
        get
        {
            int count = 0;
            foreach (InventorySlot slot in slots)
            {
                if (slot.item.id < 0)
                    count++;
            }
            return count;
        }
    }

    public bool AddItem(Item item, int amount)
    {
        if (item.id < 0) //빈 아이템 추가를 방지
            return false;
        InventorySlot slot = FindItemInInventory(item);
        //1.아이템 중첩가능한 지 여부
        //2.해당 아이템을 가진 슬롯이 없는지 여부
        if(!database.itemObjects[item.id].stackable || slot == null)
        {
            if (EmptySlotCount <= 0) // 빈 슬롯창이 없는 경우
                return false;

            if (!database.itemObjects[item.id].stackable) //추가하고자 하는 아이템이 중첩 불가인 경우
                GetEmptySlot().UpdateSlot(item, 1); //실수로 중첩불가인 아이템을 중복으로 넣을 경우 1개로 수정
            else
                GetEmptySlot().UpdateSlot(item, amount);
        }
        else
        {
            slot.AddAmount(amount);
        }

        return true;
    }

    public InventorySlot FindItemInInventory(Item item) => slots.FirstOrDefault(i => i.item.id == item.id);
    public InventorySlot GetEmptySlot() => slots.FirstOrDefault(i => i.item.id < 0);
    public bool IsContainItem(ItemObject itemObject) => slots.FirstOrDefault(i => i.item.id == itemObject.data.id) != null;

    public bool SwapItems(InventorySlot itemSlotA, InventorySlot itemSlotB)
    {
        if (itemSlotA == itemSlotB)
            return false;

        if (itemSlotB.CanPlaceInSlot(itemSlotA.ItemObject) && itemSlotA.CanPlaceInSlot(itemSlotB.ItemObject))
        {
            InventorySlot temp = new InventorySlot(itemSlotB.item, itemSlotB.amount);
            itemSlotB.UpdateSlot(itemSlotA.item, itemSlotA.amount);
            itemSlotA.UpdateSlot(temp.item, temp.amount);
            return true;
        }

        return false;
        
    }

    private void OnDestroy()
    {
        foreach(InventorySlot slot in slots)
        {
            slot.RemoveItem();
        }
    }

}
