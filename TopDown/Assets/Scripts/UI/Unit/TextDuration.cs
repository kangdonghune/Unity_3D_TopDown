using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextDuration : MonoBehaviour
{
    public float duration = 1f;

    void Start()
    {
        Managers.Resource.Destroy(gameObject, duration);
    }
}
