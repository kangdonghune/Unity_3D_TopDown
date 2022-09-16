using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager
{

    public Action KeyAction = null;
    public Action<Define.MouseEvent> MouseAction = null;
    public Action MouseInTrigger;
    public Action MouseOutTrigger;


    bool _LBpressed = false;
    bool _RBpressed = false;
    float _LBpressedTime = 0;
    float _RBpressedTime = 0;

    public void OnUpdate()
    {
        if(EventSystem.current.IsPointerOverGameObject()) // ui 버튼이 클릭되었는 지 여부 체크rr
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
                if(!_LBpressed)
                {
                    MouseAction.Invoke(Define.MouseEvent.LPointerDown);
                    _LBpressedTime = Time.time;
                }
                MouseAction.Invoke(Define.MouseEvent.LPress);
                _LBpressed = true;

            }

            else
            {
                if(_LBpressed)
                {
                    if(Time.time < _LBpressedTime + 0.2f)
                        MouseAction.Invoke(Define.MouseEvent.LClick);
                    MouseAction.Invoke(Define.MouseEvent.LPointerUp);   
                }
                _LBpressed = false;
                _LBpressedTime = 0;
            }

            if (Input.GetMouseButton(1)) // 오른쪽 0
            {
                if (!_RBpressed)
                {
                    MouseAction.Invoke(Define.MouseEvent.RPointerDown);
                    _RBpressedTime = Time.time;
                }
                MouseAction.Invoke(Define.MouseEvent.RPress);
                _RBpressed = true;

            }

            else
            {
                if (_RBpressed)
                {
                    if (Time.time < _RBpressedTime + 0.2f)
                        MouseAction.Invoke(Define.MouseEvent.RClick);
                    MouseAction.Invoke(Define.MouseEvent.RPointerUp);
                }
                _RBpressed = false;
                _RBpressedTime = 0;
            }

        }

    }

    public void Clear()
    {
        KeyAction = null;
        MouseAction = null;
    }
}
