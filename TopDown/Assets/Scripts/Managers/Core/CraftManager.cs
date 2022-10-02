using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftManager
{
    private CraftDatabase _origin;
    private CraftDatabase _database;
    private CraftDatabase _copyDatabase;
    private Dictionary<ItemObject, int> _containItems = new Dictionary<ItemObject, int>(); //현재 보유 아이템 목록
    public List<ItemObject> readyProduct = new List<ItemObject>(); //현재 제작 가능한 아이템 리스트
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

    //인벤에 아이템 추가 제거 시 호출
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

    //현재 보유한 아이템으로 만들 수 있는 아이템리스트 준비
    private void ReadyProducts()
    {
        readyProduct.Clear();
        //인벤토리를 순회하며 인벤토리 각 슬롯에 있는 아이템이 크래프트 데이터베이스의 재료에 해당한다면 데이터베이스에서 감소
        //데이터베이스에 생산품의 재료가 비어있는 상태라면 제작 가능한 아이템이라는 판정
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
        //인벤토리를 순회하며 재료 아이템 제거 장비 -> 인벤창 순
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
        //제작 완료 후 완성된 아이템을 장비 -> 인벤 칸 순으로 배치
        foreach (InventoryUI inven in Managers.UI.inventoryUIs)
        {
            if (inven.AddItemPossible(toProduce.product, 1))
                break;
        }
        return true;
    }
}
