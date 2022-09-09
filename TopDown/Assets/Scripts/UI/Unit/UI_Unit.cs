using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Unit : UI_Base
{
    public override void Init()
    {
        Managers.UI.SetCanvas(gameObject, RenderMode.WorldSpace,false);
    }
}
