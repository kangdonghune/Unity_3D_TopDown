using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
 
    public T Load<T> (string path) where T : Object
    {
        //pool Dictionary�� �ִٸ� �ű⼭ ��������
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
        //������ �׳� �ε�
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null, int count = 5)
    { 
        GameObject original = Load<GameObject>($"Prefab/{path}");

        //�ε� ���� ��
        if(original == null)
            return null;

        //�ش� ������Ʈ�� pooling ����̶�� pool��ųʸ��� �߰��ϰų� �ű⼭ �����´�
        if (original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original, parent, count).gameObject;
         
        //pooling ����� �ƴ϶�� �׳� ����
        GameObject go = Object.Instantiate(original, parent);
        go.name = original.name;

        return go;

    }

    public GameObject Instantiate(GameObject go, Vector3 position, Quaternion rotation,Transform parent = null, int count = 5)
    {
        //�ε� ���� ��
        if (go == null)
            return null;

        //�ش� ������Ʈ�� pooling ����̶�� pool��ųʸ��� �߰��ϰų� �ű⼭ �����´�
        if (go.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(go, position, rotation,parent, count).gameObject;

        //pooling ����� �ƴ϶�� �׳� ����
        GameObject gameObj = Object.Instantiate(go, position, rotation, parent);
        gameObj.name = go.name;
        return gameObj;

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
