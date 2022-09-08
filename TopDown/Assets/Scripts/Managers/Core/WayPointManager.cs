using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointManager 
{
    Dictionary<string, List<Transform>> _ways = new Dictionary<string, List<Transform>>();
    Transform _root;

    enum WayPoints
    {
        Bear,
    }

    public void Init()
    {
        if (_root == null)
        {
            _root = new GameObject { name = "@WayPoint_Root" }.transform;
            UnityEngine.Object.DontDestroyOnLoad(_root);
            Bind(typeof(WayPoints));

        }

    }

    void Bind(Type type)
    {
        string[] names = Enum.GetNames(type); //enum�� �ִ� prefab ����� string�迭�� ����
        foreach (string name in names)
        {
            string key = name + "_Way";
            _ways.Add(key, new List<Transform>()); //string �迭�� ��ȸ�ϸ� �̸��� key������ �߰�
            GameObject way = Managers.Resource.Instantiate($"WayPoints/{name}_Way"); //prefab �������� ����
            way.transform.parent = _root; // @WayPoint_Root ���Ϸ� ����
            for (int i =0; i < way.transform.childCount; i++) //������ way�� �ڽ��� ��ȸ
            {
                Transform wayPoint = way.transform.GetChild(i); //i��° �ڽ��� Ʈ������
                _ways[key].Add(wayPoint); // prefab �̸��� Ű�� ������ �߰�
            }
        }

    }

    public List<Transform> GetWays(string name)
    {
        return _ways[name];
    }

    public void Clear()
    {
        _ways.Clear();
    }
}