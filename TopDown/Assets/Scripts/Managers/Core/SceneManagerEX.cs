using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEX
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }
 
    public void LoadScene(Define.Scene scenetype)
    {
        Managers.Clear();
        SceneManager.LoadScene(GetSceneName(scenetype));
       
    }

    string GetSceneName(Define.Scene scenetype)
    {
        string name = System.Enum.GetName(typeof(Define.Scene), scenetype);
        return name;
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }

  
}
