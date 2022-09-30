using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private PlayerController _controller;

    void Start()
    {
        _controller = transform.root.GetComponent<PlayerController>();
        _controller.attackCollider = gameObject.GetComponentInChildren<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        _controller.OnExecuteAttack(other.gameObject);
    }

}
