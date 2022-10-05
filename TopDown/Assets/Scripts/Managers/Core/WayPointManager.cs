using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointManager 
{
    private List<string> wayables = new List<string>();
    Dictionary<string, List<Transform>> _ways = new Dictionary<string, List<Transform>>();
    Transform _root;

    public void Init()
    {
        if (_root == null)
        {
            _root = new GameObject { name = "@WayPoint_Root" }.transform;
            UnityEngine.Object.DontDestroyOnLoad(_root);
        }
    }

    void Bind(string name)
    {
        string key = name;
        _ways.Add(key, new List<Transform>()); //string �迭�� ��ȸ�ϸ� �̸��� key������ �߰�
        GameObject way = Managers.Resource.Instantiate($"WayPoints/{name}"); //prefab �������� ����
        way.transform.parent = _root; // @WayPoint_Root ���Ϸ� ����
        for (int i =0; i < way.transform.childCount; i++) //������ way�� �ڽ��� ��ȸ
        {
            Transform wayPoint = way.transform.GetChild(i); //i��° �ڽ��� Ʈ������
            _ways[key].Add(wayPoint); // prefab �̸��� Ű�� ������ �߰�
        }
  
    }

    public void AddWayables(string name)
    {
        if (wayables.Contains(name))
            return;
        if (!_ways.ContainsKey(name))
            Bind(name);
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
