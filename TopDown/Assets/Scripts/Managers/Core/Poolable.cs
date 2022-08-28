using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable : MonoBehaviour //해당 컴퍼넌트를 가지고 있으면 풀링, 없으면 안 한다 정도의 정보를 가진 컴퍼넌트
{
    public bool IsUsing { get; set; } = false;
}
