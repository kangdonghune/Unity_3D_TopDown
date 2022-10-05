using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Stats", menuName = "Stats System/New Character Stats New")]
public class StatsObject : ScriptableObject
{
    public Attribute[] attributes;

    public int level;
    public int exp;
    public int MaxExp = 100;

    public float HP { get; private set; }
    public float Mana { get; set; }

    public float HealthPercentage
    {
        get
        {
            float health = HP;
            float maxHealth = HP;

            foreach (Attribute attribute in attributes)
            {
                if (attribute.type == Define.UnitAttribute.HP)
                {
                    maxHealth = attribute.value.ModifiedValue;
                    break;
                }
            }
            return (maxHealth > 0 ? (health / maxHealth) : 0f);
        }
    }

    public float ManaPercentage
    {
        get
        {
            float mana = Mana;
            float maxMana = Mana;

            foreach (Attribute attribute in attributes)
            {
                if (attribute.type == Define.UnitAttribute.Mana)
                {
                    maxMana = attribute.value.ModifiedValue;
                }
            }
            return (maxMana > 0 ? (mana / maxMana) : 0f);
        }
    }

    public float ExpPercentage
    {
        get
        {
            return (MaxExp > 0 ? (exp / (float)MaxExp) : 0f);
        }

    }

    public Action<StatsObject> OnChangeStats; //���� ���� ����Ǹ� ȣ��
    public Action OnLevelChanged;

    public void InitializeAttribute(StatsObject statsObject)
    {
        attributes = statsObject.attributes;

        level = statsObject.level;
        exp = statsObject.exp;

        //���ݰ� �ʱ�ȭ �� �ִ밪���� ���簪 ����
        HP = GetModifiedValue(Define.UnitAttribute.HP);
        Mana = GetModifiedValue(Define.UnitAttribute.Mana);
    }
    
    public void InitializeAttribute()
    {
        foreach (Attribute attribute in attributes)
        {
            attribute.value = new ModifiableFloat(OnModifiedValue); //���ݿ�����Ʈ���� ���� ����� ���� �ʱ�ȭ
        }

        level = 1;
        exp = 0;


        //���ݰ� �ʱ�ȭ base�� ���� �� base + buff �� modifire�� ����. �⺻������ modifier 0�̴� base�������ʱ�ȭ
        SetBaseValue(Define.UnitAttribute.HP, 100);
        SetBaseValue(Define.UnitAttribute.Mana, 100);
        SetBaseValue(Define.UnitAttribute.Attack, 10);
        SetBaseValue(Define.UnitAttribute.AttackSpeed, 1f);
        SetBaseValue(Define.UnitAttribute.Defence, 10);
        SetBaseValue(Define.UnitAttribute.MoveSpeed, 0.1f);
        //���ݰ� �ʱ�ȭ �� �ִ밪���� ���簪 ����
        HP = GetModifiedValue(Define.UnitAttribute.HP);
        Mana = GetModifiedValue(Define.UnitAttribute.Mana);
    }

    private void OnModifiedValue(ModifiableFloat value)
    {
        OnChangeStats?.Invoke(this);
    }

    public void SetBaseValue(Define.UnitAttribute type, float value)
    {
        foreach (Attribute attribute in attributes)
        {
            if (attribute.type == type)
                attribute.value.BaseValue = value;
        }
    }

    public float GetBaseValue(Define.UnitAttribute type)
    {
        foreach (Attribute attribute in attributes)
        {
            if (attribute.type == type)
                return attribute.value.BaseValue;
        }
        return -1;
    }

    public float GetModifiedValue(Define.UnitAttribute type)
    {
        foreach (Attribute attribute in attributes)
        {
            if (attribute.type == type)
                return attribute.value.ModifiedValue;
        }
        return -1;
    }

    public float AddHP(float value)
    {
        HP += value;
        if (HP > GetModifiedValue(Define.UnitAttribute.HP))
            HP = GetModifiedValue(Define.UnitAttribute.HP);
        OnChangeStats?.Invoke(this);

        return HP;
    }

    public float SetHP(float value)
    {
        HP = value;
        if (HP > GetModifiedValue(Define.UnitAttribute.HP))
            HP = GetModifiedValue(Define.UnitAttribute.HP);
        OnChangeStats?.Invoke(this);

        return HP;
    }

    public float AddMana(float value)
    {
        Mana += value;
        if (Mana > GetModifiedValue(Define.UnitAttribute.Mana))
            Mana = GetModifiedValue(Define.UnitAttribute.Mana);
        OnChangeStats?.Invoke(this);
        return Mana;
    }

    public void LevelUp()
    {
        level++;
        MaxExp += 10;
        OnLevelChanged.Invoke();

        //���ݰ� �ʱ�ȭ base�� ���� �� base + buff �� modifire�� ����. �⺻������ modifier 0�̴� base�������ʱ�ȭ
        SetBaseValue(Define.UnitAttribute.HP, 100 + (level * 30));
        SetBaseValue(Define.UnitAttribute.Mana, 100 + (level * 30));
        SetBaseValue(Define.UnitAttribute.Attack, 10 + (level * 10));
        SetBaseValue(Define.UnitAttribute.AttackSpeed, 1f + (level * 0.1f));
        SetBaseValue(Define.UnitAttribute.Defence, 10 + (level * 10));
        SetBaseValue(Define.UnitAttribute.MoveSpeed, 0.1f);
        //���ݰ� �ʱ�ȭ �� �ִ밪���� ���簪 ����
        HP = GetModifiedValue(Define.UnitAttribute.HP);
        Mana = GetModifiedValue(Define.UnitAttribute.Mana);
    }

    public void AddExp(int value)
    {
        exp += value;
        if(exp >= MaxExp)
        {
            LevelUp();
            AddExp(-100);
        }
        OnChangeStats?.Invoke(this);
    }

}
