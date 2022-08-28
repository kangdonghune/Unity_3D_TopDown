using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Button : UI_Popup
{
 
    enum Buttons
    {
        PointButton
    }

    enum Texts
    {
        PointText,
        ScoreText
    
    }

    enum Images
    {
         ItemIcon
    }

    enum GameObjects
    {

    }

    private int _iScore = 0;


    public override void Init()
    {
        base.Init();
        
        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));

        GameObject go = GetImage((int)Images.ItemIcon).gameObject;
        BindUIEvent(go, ((PointerEventData data) => { go.transform.position = data.position; }), Define.UIEvent.Drag);

        GetButton((int)Buttons.PointButton).gameObject.BindUIEvent(OnClicked, Define.UIEvent.Click);
    }

    private void    OnClicked(PointerEventData data)
    {
        _iScore++;
        GetText((int)Texts.ScoreText).text = $"Á¡¼ö: {_iScore}Á¡";
    }

}
