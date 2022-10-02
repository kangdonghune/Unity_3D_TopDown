using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager 
{
    public List<InventoryObject> inventorys = new List<InventoryObject>();
    public List<InventoryUI> inventoryUIs = new List<InventoryUI>();

    public void Clear()
    {
        inventorys.Clear();
    }

}
