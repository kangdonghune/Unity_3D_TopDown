using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable : MonoBehaviour //�ش� ���۳�Ʈ�� ������ ������ Ǯ��, ������ �� �Ѵ� ������ ������ ���� ���۳�Ʈ
{
#if UNITY_EDITOR
    [Multiline]
    public string developmentDescription = "�ܼ��� �ش� poolable ���۳�Ʈ�� �����ϰ� �ִٸ� Ǯ�� ������� ����";
#endif 
    public bool IsUsing { get; set; } = false;
}
