using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager
{

    public Action KeyAction = null;
    public Action<Define.MouseEvent> MouseAction = null;

    bool _pressed = false;
    float _pressedTime = 0;

    public void OnUpdate()
    {
        if(EventSystem.current.IsPointerOverGameObject()) // ui 버튼이 클릭되었는 지 여부 체크
        {
            return;
        }

        if(Input.anyKey && KeyAction != null)
        {
            KeyAction.Invoke();// Invoke() 를 안해줘도 자체적으로 하지만 invoke()를 붙여주는 것으로 델리게이트라는 걸 명시
        }

        if (MouseAction != null)
        {
            if(Input.GetMouseButton(0)) // 왼쪽 0
            {
                if(!_pressed)
                {
                    MouseAction.Invoke(Define.MouseEvent.PointerDown);
                    _pressedTime = Time.time;
                }
                MouseAction.Invoke(Define.MouseEvent.Press);
                _pressed = true;

            }

            else
            {
                if(_pressed)
                {
                    if(Time.time < _pressedTime +0.2f)
                        MouseAction.Invoke(Define.MouseEvent.Click);
                    MouseAction.Invoke(Define.MouseEvent.PointerUp);   
                }
                _pressed = false;
                _pressedTime = 0;
            }

        }

    }

    public void Clear()
    {
        KeyAction = null;
        MouseAction = null;
    }
}
