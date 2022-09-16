using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType: int
{
    Weapon = 0,
    Helmet = 1,
    Chest = 2,
    Gloves = 3,
    Pants = 4,
    Boots = 5,
    Accessories = 6,
    Consumable, // ItemType ���� �ش� ������ ū ��� �Ҹ�ǰ���� ����
    Food,
    Default,

}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory System/Items/New Item")]
public class ItemObject : ScriptableObject
{
    public ItemType type;
    public bool stackable; //���ļ� ������ �� �ִ��� ����

    public Sprite icon;
    public GameObject modelPrefab; //ĳ���Ϳ��� ������ ������Ʈ�� ������

    public Item data = new Item();
    public int itemAmount;

    public List<string> boneNames = new List<string>(); //

    [TextArea(15, 20)]
    public string description;

    private void OnValidate() //������ ���� �� ȣ��
    {
        boneNames.Clear();

        if(modelPrefab == null || modelPrefab.GetComponentInChildren<SkinnedMeshRenderer>() == null)
        {
            return;
        }

        SkinnedMeshRenderer renderer = modelPrefab.GetComponentInChildren<SkinnedMeshRenderer>();
        Transform[] bones = renderer.bones;

        foreach (Transform bone in bones)
        {
            boneNames.Add(bone.name);
        }
    }

    public Item CreateItem()
    {
        Item newItem = new Item(this);
        return newItem;
    }
}
