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
        //Effect �������� �Ű������� �޾� Effct�� �Ѱ��ش�.
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
        //�� ���� �� ���� �ִ� effect�� �ִٸ� �ڷ�ƾ ���� �� ����.
        foreach(Effect effect in _effetcs)
        {
            effect.DestroyEffcet();
        }
        _effetcs.Clear();
        Object.Destroy(EffectObj);
    }

    private void DestroyCount(Effect effect,float waitTime)
    {
        //Effect�� ���� �ڷ�ƾ ����
        effect.DestroyCount(waitTime);
    }

    public void RemoveListItem(Effect effect)
    {
        _effetcs.Remove(effect);
    }
}
