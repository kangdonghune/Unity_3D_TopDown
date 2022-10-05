using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackColliderScript : MonoBehaviour
{
    private EnemyController _controller;

    void Start()
    {
        _controller = transform.root.GetComponent<EnemyController>();
        _controller.attackCollider = gameObject.GetComponentInChildren<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        _controller.OnExecuteAttack(other.gameObject);
    }
}
