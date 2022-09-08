using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable : MonoBehaviour //해당 컴퍼넌트를 가지고 있으면 풀링, 없으면 안 한다 정도의 정보를 가진 컴퍼넌트
{
#if UNITY_EDITOR
    [Multiline]
    public string developmentDescription = "단순히 해당 poolable 컴퍼넌트를 소유하고 있다면 풀링 대상으로 선택";
#endif 
    public bool IsUsing { get; set; } = false;
}
