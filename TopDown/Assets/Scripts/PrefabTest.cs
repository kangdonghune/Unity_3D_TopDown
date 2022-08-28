using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabTest : MonoBehaviour
{
    GameObject prefab;

    GameObject tank;

    void Start()
    {
        prefab = Managers.Resource.Instantiate("Tank");

        Managers.Resource.Destroy(prefab, 1.0f);

    }
}
