using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScripy : MonoBehaviour
{
    //1)���� ��� �� �� �ϳ����̶� �����ٵ� �ִ� ���(�� �� �ݶ��̴� ����)-iskinematic off
    //2)�� �� �ϳ��� isKinemaitc�� on �� ���
    private void OnCollisionEnter(Collision collision)
    {
     //   Debug.Log($"Collition: {collision.gameObject.name}");
    }

    //�� �� �ϳ��� Ʈ���� on
    //�� �� �ϳ��� �����ٵ� ���� ��
    private void OnTriggerEnter(Collider other)
    {
      //  Destroy(other.gameObject);
       // Debug.Log($"Trigger: {other.gameObject.name}"); 
    }

    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetMouseButton(0))
        {
          
        }

       
    }
}
