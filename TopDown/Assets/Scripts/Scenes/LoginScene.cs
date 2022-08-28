using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginScene : BaseScene
{
    public override void Clear()
    {
        Debug.Log("Scene Clear!");
    }

    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Login;
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            Managers.Scene.LoadScene(Define.Scene.Game);
        }
    }
}
