using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private PlayerController _controller;
    public AudioClip WeaponHitSound;

    void Start()
    {
        _controller = transform.root.GetComponent<PlayerController>();
        _controller.attackCollider = gameObject.GetComponentInChildren<Collider>();
        _controller.attackSound = WeaponHitSound;
    }

    private void OnTriggerEnter(Collider other)
    {
        _controller.OnExecuteAttack(other.gameObject);
    }

}
