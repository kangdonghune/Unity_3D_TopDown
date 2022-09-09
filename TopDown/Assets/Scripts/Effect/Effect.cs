using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public GameObject effect;

    public void DestroyCount(float waitTime)
    {
        StartCoroutine(CoDestroyParticle(effect, waitTime));
    }

    public void DestroyEffcet()
    {
        StopCoroutine("CoDestroyParticle");
        Managers.Resource.Destroy(effect);
    }

    public IEnumerator CoDestroyParticle(GameObject go, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Managers.Resource.Destroy(effect);
        Managers.Effect.RemoveListItem(this);
        Destroy(this);
    }
}
