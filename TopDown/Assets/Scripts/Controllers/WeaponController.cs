using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    void Start()
    {
        transform.root.GetComponent<PlayerController>().attackCollider = gameObject.GetComponentInChildren<Collider>();
    }
}
