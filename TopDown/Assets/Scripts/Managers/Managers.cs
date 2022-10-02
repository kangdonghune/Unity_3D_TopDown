using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static bool applicationIsQuitting = false;
    static Managers s_Instance = null; // 싱글턴 유일성 보장
    static Managers Instance { get { Init(); return s_Instance; } }

    #region Contents
    //GameManager m_gameManager = new GameManager();
    //public static GameManager Game { get { return Instance.m_gameManager; } }
    #endregion

    #region Core
    InputManager m_input = new InputManager();
    public  static  InputManager Input { get { return Instance?.m_input; } }// 스테틱 은 스테틱만 쓸 수 있으니 바로 접근 불가.

    PoolManager m_pool = new PoolManager();
    public static PoolManager Pool { get { return Instance?.m_pool; } }

    ResourceManager m_Resource = new ResourceManager();
    public  static  ResourceManager Resource { get { return Instance?.m_Resource; } }

    SceneManagerEX m_Scene = new SceneManagerEX();
    public static SceneManagerEX Scene { get { return Instance?.m_Scene; } }

    SoundManager m_sound = new SoundManager();
    public static SoundManager Sound { get { return Instance?.m_sound; } }

    UI_Manager m_ui = new UI_Manager();
    public  static  UI_Manager UI { get { return Instance?.m_ui; } }

    WayPointManager m_way = new WayPointManager();
    public static WayPointManager Way { get { return Instance?.m_way; } }

    EffectManager m_effect = new EffectManager();
    public static EffectManager Effect { get { return Instance?.m_effect; } }

    CraftManager m_craft = new CraftManager();
    public static CraftManager Craft { get { return Instance?.m_craft; } }
    #endregion

    void Start()
    {
        Init();
    }

    void Update()
    {
        m_input.OnUpdate();
    }

    static void Init()
    {
        if (applicationIsQuitting)
            return;

        if (s_Instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }
            DontDestroyOnLoad(go);
            s_Instance = go.GetComponent<Managers>();
            s_Instance.m_effect.Init();
            s_Instance.m_sound.Init();
            s_Instance.m_pool.Init();
            s_Instance.m_way.Init();
            s_Instance.m_craft.Init();
        }

    }
     
    public static void Clear()
    {
        Sound.Clear();
        Input.Clear();
        Scene.Clear();
        UI.Clear();
        Way.Clear();
        Effect.Clear();
        Pool.Clear(); //다른 컴퍼넌트에서 pool된 애들을 사용할 수 있으니 마지막으로 clear
    }

    public void OnDestroy()
    {
        applicationIsQuitting = true;
    }
}
