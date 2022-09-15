using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInstance
{
    public List<Transform> itemTransforms = new List<Transform>();
  
    
    public void Destroy()
    {
        foreach (Transform item in itemTransforms)
        {
            Managers.Resource.Destroy(item.gameObject);
        }
    }
}
