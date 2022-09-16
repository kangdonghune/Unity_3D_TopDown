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
    Consumable, // ItemType 값이 해당 값보다 큰 경우 소모품으로 판정
    Food,
    Default,

}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory System/Items/New Item")]
public class ItemObject : ScriptableObject
{
    public ItemType type;
    public bool stackable; //겹쳐서 소유할 수 있는지 여부

    public Sprite icon;
    public GameObject modelPrefab; //캐릭터에게 부착될 오브젝트의 프리팹

    public Item data = new Item();
    public int itemAmount;

    public List<string> boneNames = new List<string>(); //

    [TextArea(15, 20)]
    public string description;

    private void OnValidate() //데이터 변경 시 호출
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
