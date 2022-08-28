using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inven : UI_Scene
{

    enum GameObjects
    {
        GridPanel,
    }

    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));

        GameObject gridpanel = Get<GameObject>((int)GameObjects.GridPanel);
        foreach (Transform child in gridpanel.transform)
        {
            Managers.Resource.Destroy(child.gameObject);
        }

        for(int i = 0; i < 8; i++)
        {
            GameObject item = Managers.UI.MakeSubItem<UI_Inven_Item>(gridpanel.transform).gameObject;
            UI_Inven_Item inven_Item = item.GetOrAddComponent<UI_Inven_Item>();
            inven_Item.SetInfo($"집행검 {i} 호");
        }
    }

    void Update()
    {
        
    }
}
