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
    public GameObject slotUI;//������ ǥ���ϴ� ui ������Ʈ
    [NonSerialized]
    public Action<InventorySlot> OnPreUpdate;
    [NonSerialized]
    public Action<InventorySlot> OnPostUpdate;

    public Item item; //���� ������Ʈ �� ������ ����
    public int amount;//������ ����

    //���� �����ۿ�����Ʈ
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

        this.item = item; //������ ����(ex: ���� ��)
        this.amount = amount;//������ stackable�� true�� ��� 2 �̻� ����

        OnPostUpdate?.Invoke(this);
    }

    public bool CanPlaceInSlot(ItemObject itemObject)
    {
        //1.���Կ� ������ ���� Ÿ�Կ� ������ ���ų�(���ĭX)
        //2.���� �ְ��� �ϴ� �������� ���ų� ������Ʈ �� �������� Ÿ���� �������� ���� ���(�� �κ��丮����) = ������ ���� ���
        if (allowedItems.Length <= 0 || itemObject == null || itemObject.data.id < 0)
            return true; //���ο� �������� ������ �� �ִ�.

        foreach (ItemType type in allowedItems)
        {
            if (itemObject.type == type) //���� Ÿ���� �´ٸ�
                return true;
        }

        return false;

    }
}


