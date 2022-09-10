using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType: int
{
    Helmet = 0,
    Chest = 1,
    Pants = 2,
    Boots = 3,
    Gloves = 4,
    Weapon = 5,
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
