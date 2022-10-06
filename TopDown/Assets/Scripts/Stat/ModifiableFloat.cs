using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ModifiableFloat
{
    [NonSerialized]
    private float baseValue; //�����ڰ� ���Ƿ� ���� ������ ��
    [SerializeField]
    private float modifiedValue; // base value + ������ ���� ���ݿ� ���� �����Ǵ� ��

    public float BaseValue { get => baseValue; set { baseValue = value; UpdateModifiedValue(); } }
    public float ModifiedValue { get => modifiedValue; set => modifiedValue = value; }

    private event Action<ModifiableFloat> OnModifiedValue;

    private List<IModifier> modifiers = new List<IModifier>();

    public ModifiableFloat(float value, Action<ModifiableFloat> method = null)
    {
        ModifiedValue = value;
        RegisterModEvent(method); //���� �� �̺�Ʈ �߰�
    }

    public ModifiableFloat(Action<ModifiableFloat> method = null)
    {
        ModifiedValue = baseValue;
        RegisterModEvent(method); //���� �� �̺�Ʈ �߰�
    }

    public void RegisterModEvent(Action<ModifiableFloat> method) //�̺�Ʈ �߰�
    {
        if(method != null)
        {
            OnModifiedValue -= method;
            OnModifiedValue += method;
        }
    }

    public void UnregisterModEvent(Action<ModifiableFloat> method) //�̺�Ʈ ����
    {
        if(method != null)
        {
            OnModifiedValue -= method; 
        }
    }

    private void UpdateModifiedValue() //baseValue ���� �� OnModifiedValue ȣ���Ͽ� ����
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
