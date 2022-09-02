using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{


    void Start()
    {
        Init();
    }

    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.Game;
        //Managers.UI.ShowSceneUI<UI_Inven>();

        gameObject.GetOrAddComponent<CursorController>();
        GameObject player = Managers.Game.Spawn(Define.WorldObject.Player, "Arisa");
        Managers.Game.Spawn(Define.WorldObject.Monster, "Bear");
        Camera.main.gameObject.GetOrAddComponent<CameraController>().SetTarget(player);

        //GameObject go = new GameObject { name = "SpawningPool" };
        //SpawningPool spawningPool = go.GetOrAddComponent<SpawningPool>();
        //spawningPool.SetKeepMonsterCount(5);
    }

    public override void Clear()
    {
        throw new System.NotImplementedException();
    }
}
