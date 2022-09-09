using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EffectManager 
{
    private List<Effect> _effetcs = new List<Effect>();
    private GameObject EffectObj;

    public void Init()
    {
        EffectObj = GameObject.Find("@Effect");
        if (EffectObj == null)
        {
            EffectObj = new GameObject { name = "@Effect" };
        }
    }

    public void Instantiate(GameObject EffectPrefab, Vector3 position, Quaternion rotation)
    {
        if (EffectPrefab == null)
            return;
        //Effect 프리팹을 매개변수로 받아 Effct에 넘겨준다.
        GameObject VFX = Managers.Resource.Instantiate(EffectPrefab, position, rotation);
        Effect effect = EffectObj.AddComponent<Effect>();
        effect.effect = VFX;
        _effetcs.Add(effect);

        ParticleSystem particleSystem = VFX.GetComponent<ParticleSystem>();
        if (particleSystem)
        {
            DestroyCount(effect,particleSystem.main.duration);
        }
        else
        {
            ParticleSystem childParticleSystem = VFX.transform.GetChild(0).GetComponent<ParticleSystem>();
            if (childParticleSystem)
            {
                DestroyCount(effect, childParticleSystem.main.duration);
            }
        }

    }

    public void Clear()
    {
        //씬 종료 시 남아 있는 effect가 있다면 코루틴 종료 후 삭제.
        foreach(Effect effect in _effetcs)
        {
            effect.DestroyEffcet();
        }
        _effetcs.Clear();
        Object.Destroy(EffectObj);
    }

    private void DestroyCount(Effect effect,float waitTime)
    {
        //Effect의 삭제 코루틴 실행
        effect.DestroyCount(waitTime);
    }

    public void RemoveListItem(Effect effect)
    {
        _effetcs.Remove(effect);
    }
}
