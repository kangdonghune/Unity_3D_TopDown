using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
 
    public T Load<T> (string path) where T : Object
    {
        //pool Dictionary에 있다면 거기서 가져오고
        if(typeof(T) == typeof(GameObject))
        {
            string name = path;
            int idx = name.LastIndexOf("/");
            if(idx > 0)
                name =name.Substring(idx+1);

            GameObject go = Managers.Pool.GetOriginal(name);
            if (go != null)
                return go as T;
        }
        if (Resources.Load<T>(path) == null)
            Debug.LogError($"Failed to load prefab: {path}");
        //없으면 그냥 로드
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null, int count = 5)
    { 
        GameObject original = Load<GameObject>($"Prefab/{path}");

        //로드 실패 시
        if(original == null)
            return null;

        //해당 오브젝트가 pooling 대상이라면 pool딕셔너리에 추가하거나 거기서 가져온다
        if (original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original, parent, count).gameObject;
         
        //pooling 대상이 아니라면 그냥 샹송
        GameObject go = Object.Instantiate(original, parent);
        go.name = original.name;

        return go;

    }

    public GameObject Instantiate(GameObject go, Vector3 position, Quaternion rotation,Transform parent = null, int count = 5)
    {
        //로드 실패 시
        if (go == null)
            return null;

        //해당 오브젝트가 pooling 대상이라면 pool딕셔너리에 추가하거나 거기서 가져온다
        if (go.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(go, position, rotation,parent, count).gameObject;

        //pooling 대상이 아니라면 그냥 생성
        GameObject gameObj = Object.Instantiate(go, position, rotation, parent);
        gameObj.name = go.name;
        return gameObj;

    }


    public void Destroy(GameObject go, float time = 0.0f)
    {
        if (go == null)
            return;

        //바로 디스트로이 하는 게 아니라 
        //만약에 풀링이 필요한 오브젝트면 풀링 매니저한테 위탁

        Poolable poolable = go.GetComponent<Poolable>();
        if(poolable != null)
        {
            Managers.Pool.Push(poolable); // poolable 보유한 오브젝트면 비활성한 상태로 pool_root에 추가
            return;
        }

        Object.Destroy(go, time);
    }

}
