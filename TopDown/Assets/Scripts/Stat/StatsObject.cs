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

    public Action<StatsObject> OnChangeStats; //스텟 값이 변경되면 호출
    public Action OnLevelChanged;

    public void InitializeAttribute(StatsObject statsObject)
    {
        attributes = statsObject.attributes;

        level = statsObject.level;
        exp = statsObject.exp;

        //스텟값 초기화 및 최대값으로 현재값 수정
        HP = GetModifiedValue(Define.UnitAttribute.HP);
        Mana = GetModifiedValue(Define.UnitAttribute.Mana);
    }
    
    public void InitializeAttribute()
    {
        foreach (Attribute attribute in attributes)
        {
            attribute.value = new ModifiableFloat(OnModifiedValue); //스텟오브젝트에서 값이 변경된 경우로 초기화
        }

        level = 1;
        exp = 0;


        //스텟값 초기화 base값 수정 시 base + buff 가 modifire에 적용. 기본적으론 modifier 0이니 base값으로초기화
        SetBaseValue(Define.UnitAttribute.HP, 100);
        SetBaseValue(Define.UnitAttribute.Mana, 100);
        SetBaseValue(Define.UnitAttribute.Attack, 10);
        SetBaseValue(Define.UnitAttribute.AttackSpeed, 1f);
        SetBaseValue(Define.UnitAttribute.Defence, 10);
        SetBaseValue(Define.UnitAttribute.MoveSpeed, 0.1f);
        //스텟값 초기화 및 최대값으로 현재값 수정
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

        //스텟값 초기화 base값 수정 시 base + buff 가 modifire에 적용. 기본적으론 modifier 0이니 base값으로초기화
        SetBaseValue(Define.UnitAttribute.HP, 100 + (level * 30));
        SetBaseValue(Define.UnitAttribute.Mana, 100 + (level * 30));
        SetBaseValue(Define.UnitAttribute.Attack, 10 + (level * 10));
        SetBaseValue(Define.UnitAttribute.AttackSpeed, 1f + (level * 0.1f));
        SetBaseValue(Define.UnitAttribute.Defence, 10 + (level * 10));
        SetBaseValue(Define.UnitAttribute.MoveSpeed, 0.1f);
        //스텟값 초기화 및 최대값으로 현재값 수정
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
