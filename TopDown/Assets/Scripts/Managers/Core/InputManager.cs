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
        if(EventSystem.current.IsPointerOverGameObject()) // ui ��ư�� Ŭ���Ǿ��� �� ���� üũrr
        {
            return;
        }

        if(Input.anyKey && KeyAction != null)
        {
            KeyAction.Invoke();// Invoke() �� �����൵ ��ü������ ������ invoke()�� �ٿ��ִ� ������ ��������Ʈ��� �� ���
        }

        if (MouseAction != null)
        {
            if(Input.GetMouseButton(0)) // ���� 0
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

            if (Input.GetMouseButton(1)) // ������ 0
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
