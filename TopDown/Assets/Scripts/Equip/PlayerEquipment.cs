using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public InventoryObject equipment;

    private EquipmentCombiner combiner;

    private ItemInstance[] itemInstances = new ItemInstance[6];


    public ItemObject[] defalutItemObjects = new ItemObject[6];

    private void Awake()
    {
        combiner = new EquipmentCombiner(gameObject);

        for (int i = 0; i < equipment.slots.Length; i++)
        {
            equipment.slots[i].OnPreUpdate -= OnRemoveItem;
            equipment.slots[i].OnPostUpdate -= OnEquipItem;
            equipment.slots[i].OnPreUpdate += OnRemoveItem;
            equipment.slots[i].OnPostUpdate += OnEquipItem;
        }
    }


    private void Start()
    {
        for(int i = 0; i < itemInstances.Length; i++)
        {
            if (defalutItemObjects[i] == null)
            {
                equipment.slots[i].RemoveItem();
                continue;
            }    
            if(equipment.slots[i].CanPlaceInSlot(defalutItemObjects[i])) //����Ʈ �������� �ش� ���Կ� ���� ������ Ÿ������ üũ
                equipment.slots[i].UpdateSlot(defalutItemObjects[i].data, 1);
        }
    }

    private void OnEquipItem(InventorySlot slot)
    {
        ItemObject itemObject = slot.ItemObject;
        if(itemObject == null)
        {
            //��� �κ��丮�� ����ִٸ� �ش� ������ ù��° ��� Ÿ������ ����Ʈ ������ ����
            EquipDefalutItem(slot.allowedItems[0]);
            return;
        }

        int index = (int)slot.allowedItems[0];

        switch(slot.allowedItems[0])
        {
            case ItemType.Weapon:
                itemInstances[index] = EquipMeshItem(itemObject);
                break;
            case ItemType.Helmet:
            case ItemType.Chest:
            case ItemType.Gloves:
            case ItemType.Pants:
            case ItemType.Boots:
                itemInstances[index] = EquipSkinnedItem(itemObject);
                break;
            default:
                break;
        }

    }


    private void EquipDefalutItem(ItemType type)
    {
        int index = (int)type;

        ItemObject itemObject = defalutItemObjects[index];
        switch(type)
        {
            case ItemType.Weapon:
                itemInstances[index] = EquipMeshItem(itemObject);
                break;
            case ItemType.Helmet:
            case ItemType.Chest:
            case ItemType.Gloves:
            case ItemType.Pants:
            case ItemType.Boots:
                itemInstances[index] = EquipSkinnedItem(itemObject);
                break;
            default:
                break;
        }
    }

    private ItemInstance EquipSkinnedItem(ItemObject itemObject)
    {
        if (itemObject == null)
            return null;

        Transform itemTransform = combiner.AddLimb(itemObject.modelPrefab, itemObject.boneNames);

        ItemInstance instance = new ItemInstance();
        if(itemTransform != null)
        {
            instance.itemTransforms.Add(itemTransform);
            return instance;
        }

        return null;

    }

    private ItemInstance EquipMeshItem(ItemObject itemObject)
    {
        if (itemObject == null)
            return null;

        Transform[] itemTransforms = combiner.AddMesh(itemObject.modelPrefab);

        if(itemTransforms.Length > 0)
        {
            ItemInstance instance = new ItemInstance();
            instance.itemTransforms.AddRange(itemTransforms.ToList<Transform>());
            return instance;
        }

        return null;
    }

    //private void OnDestroy()
    //{
    //    foreach(ItemInstance item in itemInstances)
    //    {
    //        item?.Destroy();
    //    }
    //}

    private void OnRemoveItem(InventorySlot slot)
    {
        ItemObject itemObject = slot.ItemObject;
        if (itemObject == null)
        {
            RemoveItemBy(slot.allowedItems[0]);
            return;
        }

        if(slot.ItemObject.modelPrefab != null)
        {
            RemoveItemBy(slot.allowedItems[0]);
            return;
        }
    }

    private void RemoveItemBy(ItemType type)
    {
        int index = (int)type;
        if(itemInstances[index] != null)
        {
            itemInstances[index].Destroy();
            itemInstances[index] = null;
        }
    }

}
