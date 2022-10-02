using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftManager
{
    private CraftDatabase _origin;
    private CraftDatabase _database;
    private CraftDatabase _copyDatabase;
    private Dictionary<ItemObject, int> _containItems = new Dictionary<ItemObject, int>(); //���� ���� ������ ���
    public List<ItemObject> readyProduct = new List<ItemObject>(); //���� ���� ������ ������ ����Ʈ
    public System.Action OnUpdateEvent;

    public void Init()
    {
        _origin = Resources.Load<CraftDatabase>("Prefab/Data/Craft/CraftDatabase");
        _database = Object.Instantiate<CraftDatabase>(_origin);
    }

    public void CopyDatabase()
    {
        _copyDatabase = new CraftDatabase();
        _copyDatabase.Contents = new List<CraftObject>();
        foreach (CraftObject craftObject in _database.Contents)
        {
            _copyDatabase.Contents.Add(new CraftObject(craftObject.ingredients, craftObject.product));
        }
    }

    //�κ��� ������ �߰� ���� �� ȣ��
    public void UpdateContainItems()
    {
        _containItems.Clear();
        CopyDatabase();
        foreach (InventoryObject inven in Managers.UI.inventorys)
        {
            foreach (InventorySlot slot in inven.slots)
            {
                if (slot.ItemObject == null)
                    continue;

                if(!_containItems.ContainsKey(slot.ItemObject))
                {
                    _containItems.Add(slot.ItemObject, slot.amount);
                    continue;
                }
                _containItems[slot.ItemObject] += slot.amount;
            }
        }
        ReadyProducts();
        OnUpdateEvent.Invoke();
    }

    //���� ������ ���������� ���� �� �ִ� �����۸���Ʈ �غ�
    private void ReadyProducts()
    {
        readyProduct.Clear();
        //�κ��丮�� ��ȸ�ϸ� �κ��丮 �� ���Կ� �ִ� �������� ũ����Ʈ �����ͺ��̽��� ��ῡ �ش��Ѵٸ� �����ͺ��̽����� ����
        //�����ͺ��̽��� ����ǰ�� ��ᰡ ����ִ� ���¶�� ���� ������ �������̶�� ����
        foreach(CraftObject craftObject in _copyDatabase.Contents)
        {
            List<Craftingredient> deletes = new List<Craftingredient>();
            foreach (Craftingredient ingredient in craftObject.ingredients)
            {
                if(_containItems.ContainsKey(ingredient.ingredient))
                {
                    ingredient.amount -= _containItems[ingredient.ingredient];
                    if (ingredient.amount <= 0)
                        deletes.Add(ingredient);
                }
            }
            foreach(Craftingredient delete in deletes)
            {
                craftObject.ingredients.Remove(delete);
            }
            if (craftObject.ingredients.Count == 0)
            {
                foreach (CraftObject craftorigin in _database.Contents)
                {
                    if (craftorigin.product == craftObject.product)
                    {
                        readyProduct.Add(craftorigin.product);
                        break;
                    }
                }
            }
        }
    }

    public bool Craft(ItemObject product)
    {
        CopyDatabase();
        CraftObject toProduce = null;
        foreach(CraftObject craftObject in _copyDatabase.Contents)
        {
            if(craftObject.product == product)
            {
                toProduce = craftObject;
                break;
            }
        }
        if (toProduce == null)
            return false;
        //�κ��丮�� ��ȸ�ϸ� ��� ������ ���� ��� -> �κ�â ��
        foreach (InventoryObject inven in Managers.UI.inventorys)
        {
            for(int i =0; i < inven.slots.Length; i++)
            {
                foreach (Craftingredient ingredient in toProduce.ingredients)
                {
                    if (ingredient.amount <= 0)
                        continue;
                    if(inven.slots[i].ItemObject == ingredient.ingredient)
                    {
                        int tempAmount = inven.slots[i].amount;
                        inven.slots[i].RemoveAmount(ingredient.amount);
                        ingredient.amount -= tempAmount;
                    }
                }
            }
        }
        //���� �Ϸ� �� �ϼ��� �������� ��� -> �κ� ĭ ������ ��ġ
        foreach (InventoryUI inven in Managers.UI.inventoryUIs)
        {
            if (inven.AddItemPossible(toProduce.product, 1))
                break;
        }
        return true;
    }
}
