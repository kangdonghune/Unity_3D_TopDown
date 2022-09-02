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
        string[] names = Enum.GetNames(type); //enum에 있는 prefab 목록을 string배열로 추출
        foreach (string name in names)
        {
            string key = name + "_Way";
            _ways.Add(key, new List<Transform>()); //string 배열을 순회하며 이름을 key값으로 추가
            GameObject way = Managers.Resource.Instantiate($"WayPoints/{name}_Way"); //prefab 폴더에서 생성
            way.transform.parent = _root; // @WayPoint_Root 산하로 모음
            for (int i =0; i < way.transform.childCount; i++) //생성된 way의 자식을 순회
            {
                Transform wayPoint = way.transform.GetChild(i); //i번째 자식의 트랜스폼
                _ways[key].Add(wayPoint); // prefab 이름의 키에 벨류로 추가
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
