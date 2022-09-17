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
    public InterfaceType type; //���â����, �κ�â����, ���������� ����

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
        if (item.id < 0) //�� ������ �߰��� ����
            return false;
        InventorySlot slot = FindItemInInventory(item);
        //1.������ ��ø������ �� ����
        //2.�ش� �������� ���� ������ ������ ����
        if(!database.itemObjects[item.id].stackable || slot == null)
        {
            if (EmptySlotCount <= 0) // �� ����â�� ���� ���
                return false;

            if (!database.itemObjects[item.id].stackable) //�߰��ϰ��� �ϴ� �������� ��ø �Ұ��� ���
                GetEmptySlot().UpdateSlot(item, 1); //�Ǽ��� ��ø�Ұ��� �������� �ߺ����� ���� ��� 1���� ����
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
