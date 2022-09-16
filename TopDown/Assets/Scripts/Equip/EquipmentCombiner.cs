using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentCombiner
{
    private readonly Dictionary<int, Transform> rootBoneDictionary = new Dictionary<int, Transform>();

    private readonly Transform transform;

    private string WeaponString = "OneHandWeapon";
    private Transform WeaponTransform;


    public EquipmentCombiner(GameObject rootGo)
    {
        transform = rootGo.transform;
        TraverseHierachy(transform);
    }

    //skined 메쉬가 필요한 아이템이 올 때 스킨드 메쉬의 본 네임과 현재 캐릭터의 본 네임을 비교해서 재설정
    public Transform AddLimb(GameObject itemObject, List<string> boneNames)
    {
        Transform limb = ProcessBoneObject(itemObject.GetComponentInChildren<SkinnedMeshRenderer>(), boneNames);
        limb.SetParent(transform);

        return limb;
    }

    private void TraverseHierachy(Transform root)
    {
        foreach (Transform child in root)
        {
            if (child.name == "OneHandWeapon")
            {
                WeaponTransform = child;
            }
            rootBoneDictionary.Add(child.name.GetHashCode(), child);
            TraverseHierachy(child);
        }
    }

    private Transform ProcessBoneObject(SkinnedMeshRenderer renderer, List<string> boneNames)
    {
        Transform itemTransform = new GameObject().transform;
        SkinnedMeshRenderer meshRenderer = itemTransform.gameObject.AddComponent<SkinnedMeshRenderer>();
        Transform[] boneTransforms = new Transform[boneNames.Count];
        for(int i = 0; i <boneNames.Count; i++)
        {
            boneTransforms[i] = rootBoneDictionary[boneNames[i].GetHashCode()];
        }
        meshRenderer.bones = boneTransforms;
        meshRenderer.sharedMesh = renderer.sharedMesh;
        meshRenderer.sharedMaterial = renderer.sharedMaterial;

        return itemTransform;
    }

    public Transform[] AddMesh(GameObject itemObject)
    {
        Transform[] itemTransforms = ProcessMeshObject(itemObject.GetComponentsInChildren<MeshRenderer>());
        return itemTransforms;
    }

    private Transform[] ProcessMeshObject(MeshRenderer[] meshRenderers)
    {
        List<Transform> itemTransforms = new List<Transform>();
        foreach(MeshRenderer renderer in meshRenderers)
        {
            if(renderer.transform.parent != null)
            {
                Transform parent = rootBoneDictionary[renderer.transform.parent.name.GetHashCode()];
                GameObject itemGo = GameObject.Instantiate(renderer.gameObject, parent);
                itemTransforms.Add(itemGo.transform);
            }
            else
            {
                Transform parent = rootBoneDictionary[renderer.transform.name.GetHashCode()];
                GameObject itemGo = GameObject.Instantiate(renderer.gameObject, parent);
                itemTransforms.Add(itemGo.transform);
            }

        }

        return itemTransforms.ToArray();
    }

}
