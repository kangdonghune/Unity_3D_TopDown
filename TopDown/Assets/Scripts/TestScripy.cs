using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScripy : MonoBehaviour
{
    //1)나와 상대 둘 중 하나만이라도 리짓바디가 있는 경우(둘 다 콜라이더 보유)-iskinematic off
    //2)둘 중 하나만 isKinemaitc이 on 인 경우
    private void OnCollisionEnter(Collision collision)
    {
     //   Debug.Log($"Collition: {collision.gameObject.name}");
    }

    //둘 중 하나만 트리거 on
    //둘 중 하나는 리짓바디가 있을 것
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
