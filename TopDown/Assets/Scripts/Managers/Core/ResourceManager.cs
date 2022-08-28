using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
 
    public T Load<T> (string path) where T : Object
    {
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
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null, int count = 5)
    { 
        GameObject original = Load<GameObject>($"Prefab/{path}");

        if(original == null)
            return null;

        if (original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original, parent, count).gameObject;
         

        GameObject go = Object.Instantiate(original, parent);
        go.name = original.name;

        return go;

    }

    public void Destroy(GameObject go, float time = 0.0f)
    {
        if (go == null)
            return;

        //�ٷ� ��Ʈ���� �ϴ� �� �ƴ϶� 
        //���࿡ Ǯ���� �ʿ��� ������Ʈ�� Ǯ�� �Ŵ������� ��Ź

        Poolable poolable = go.GetComponent<Poolable>();
        if(poolable != null)
        {
            Managers.Pool.Push(poolable); // poolable ������ ������Ʈ�� ��Ȱ���� ���·� pool_root�� �߰�
            return;
        }

        Object.Destroy(go, time);
    }

}
