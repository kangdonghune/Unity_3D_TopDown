using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ModifiableFloat
{
    [NonSerialized]
    private float baseValue; //개발자가 임의로 수정 가능한 값
    [SerializeField]
    private float modifiedValue; // base value + 아이템 버프 스텟에 의해 변동되는 값

    public float BaseValue { get => baseValue; set { baseValue = value; UpdateModifiedValue(); } }
    public float ModifiedValue { get => modifiedValue; set => modifiedValue = value; }

    private event Action<ModifiableFloat> OnModifiedValue;

    private List<IModifier> modifiers = new List<IModifier>();

    public ModifiableFloat(float value, Action<ModifiableFloat> method = null)
    {
        ModifiedValue = value;
        RegisterModEvent(method); //생성 시 이벤트 추가
    }

    public ModifiableFloat(Action<ModifiableFloat> method = null)
    {
        ModifiedValue = baseValue;
        RegisterModEvent(method); //생성 시 이벤트 추가
    }

    public void RegisterModEvent(Action<ModifiableFloat> method) //이벤트 추가
    {
        if(method != null)
        {
            OnModifiedValue -= method;
            OnModifiedValue += method;
        }
    }

    public void UnregisterModEvent(Action<ModifiableFloat> method) //이벤트 제거
    {
        if(method != null)
        {
            OnModifiedValue -= method; 
        }
    }

    private void UpdateModifiedValue() //baseValue 갱신 시 OnModifiedValue 호출하여 갱신
    {
        float valueToAdd = 0f;
        foreach (IModifier modifier in modifiers)
        {
            modifier.AddValue(ref valueToAdd);
        }

        ModifiedValue = baseValue + valueToAdd;
        OnModifiedValue.Invoke(this);
    }

    public void AddModifier(IModifier modifier)
    {
        modifiers.Add(modifier);

        UpdateModifiedValue();
    }

    public void RemoveModifier(IModifier modifier)
    {
        modifiers.Remove(modifier);
        UpdateModifiedValue();
    }



}
