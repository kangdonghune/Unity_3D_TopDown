using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager 
{
    int _order = 10; // ������� �ֱٿ� ����� ����

    Stack<UI_Popup> _popStack = new Stack<UI_Popup>();
    UI_Scene _sceneUI = null;

    public GameObject Root
    {
        get
        {
            GameObject UI_Root = GameObject.Find("@UI_Root");
            if (UI_Root == null)
            {
                UI_Root = new GameObject("@UI_Root");
            }
            return UI_Root;
        }
    }

    public GameObject PopupUIRoot
    {
        get
        {
            GameObject popuproot = GameObject.Find("@UI_Popup");
            if (popuproot == null)
            {
                popuproot = new GameObject { name = "@UI_Popup" };
                popuproot.transform.SetParent(Root.transform);
            }

            return popuproot;
        }
    }

    public GameObject SceneUIRoot
    {
        get
        {
            GameObject sceneroot = GameObject.Find("@UI_Scene");
            if (sceneroot == null)
            {
                sceneroot = new GameObject { name = "@UI_Scene" };
                sceneroot.transform.SetParent(Root.transform);
            }
            return sceneroot;
        }
    }


    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true; //ĵ���� �ȿ� ĵ������ ��ø�� �� �� �θ� � ���� ������ ���θ��� ���ÿ��� ���� ������ ����


        if (sort) //�˾� ������
        {
            canvas.sortingOrder = (_order);
            _order++;
        }

        else // �� ������
        {
            canvas.sortingOrder = 0;
        }
        

    }

    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");
        T sceneUI = Util.GetOrAddComponent<T>(go);
        _sceneUI = sceneUI;

        go.transform.SetParent(SceneUIRoot.transform);

 
        return sceneUI;
    }

    public T ShowPopupUI<T>(string name =null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");
        go.transform.SetParent(PopupUIRoot.transform);
        T popup = Util.GetOrAddComponent<T>(go);
        _popStack.Push(popup);

        return popup;
    }

    public T MakeWorldSpaceUI<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/WorldSpace/{name}");

        if (go == null)
            Debug.LogError($"Make WorldSpaceUI Failed: {name}");

        if (parent != null)
            go.transform.SetParent(parent);

        Canvas canvas = go.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;
         
        return go.GetOrAddComponent<T>();

    }

    public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/SubItem/{name}");

        if (parent != null)
            go.transform.SetParent(parent);

        return go.GetOrAddComponent<T>();
      
    }

    public void ClosePopupUI(UI_Popup popup)
    {
        if (_popStack.Count == 0)
            return;

        if(_popStack.Peek() != popup)
        {
            Debug.LogError("Close Popup Failed");
            return;
        }

        ClosePopupUI();
    }

    public void ClosePopupUI()
    {
        if (_popStack.Count == 0)
            return;

        UI_Popup popup = _popStack.Pop(); //���� �ֱٿ� ��� â
        Managers.Resource.Destroy(popup.gameObject);
        popup = null;

        _order--;
    }

    public void CloseAllPopupUI()
    {
        while (_popStack.Count > 0)
            ClosePopupUI();
    }

    public void Clear()
    {
        CloseAllPopupUI();
        _sceneUI = null;
    }

}
