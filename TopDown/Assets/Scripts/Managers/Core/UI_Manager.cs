using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager 
{
    int _order = 10; // 현재까지 최근에 사용한 오더

    Stack<UI_Popup> _popStack = new Stack<UI_Popup>();

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


    public void SetCanvas(GameObject go, RenderMode rendermode,bool sort = true)
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = rendermode;
        canvas.overrideSorting = true; //캔버스 안에 캔버스가 중첩될 때 그 부모가 어떤 값을 가지든 본인만의 소팅오더 값을 가지는 설정


        if (sort) //팝업 유아이
        {
            canvas.sortingOrder = (_order);
            _order++;
        }

        else // 씬 유아이
        {
            canvas.sortingOrder = 0;
        }
        

    }

    public T CreateUnitUI<T>(string name = null, Transform parent = null) where T : UI_Unit
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;
        //기본적으론 풀링과정에서 고유성을 보장하기 위해 디펄트값은 1개로
        GameObject go = Managers.Resource.Instantiate($"UI/Unit/{name}", parent);
        T unitUI = Util.GetOrAddComponent<T>(go);
        //UI가 카메라 쪽 바라보도록 추가
        Util.GetOrAddComponent<CameraFacing>(go);
        return unitUI;
    }

    public T CreateSceneUI<T>(string name = null, Transform parent = null, int poolCount = 1) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;
        //기본적으론 풀링과정에서 고유성을 보장하기 위해 디펄트값은 1개로
        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}", parent, poolCount);
        T sceneUI = Util.GetOrAddComponent<T>(go);
        if (parent == null)
            go.transform.SetParent(SceneUIRoot.transform);
        return sceneUI;
    }

    public void ShowSceneUI<T>(T sceneUI) where T: UI_Scene
    {
        if (sceneUI == null || sceneUI.gameObject.activeSelf == true)
            return;

        sceneUI.gameObject.SetActive(true);
    }

    public void CloseSceneUI<T>(T sceneUI) where T : UI_Scene
    {
        if (sceneUI == null)
            return;

        sceneUI.gameObject.SetActive(false);       
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

        UI_Popup popup = _popStack.Pop(); //가장 최근에 띄운 창
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
    }

}
