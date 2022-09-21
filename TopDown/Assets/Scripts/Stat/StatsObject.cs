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
                if (attribute.type == Define.CharacterAttribute.HP)
                {
                    maxHealth = attribute.value.ModifiedValue;
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
                if (attribute.type == Define.CharacterAttribute.Mana)
                {
                    maxMana = attribute.value.ModifiedValue;
                }
            }
            return (maxMana > 0 ? (mana / maxMana) : 0f);
        }
    }

    public Action<StatsObject> OnChangeStats; //스텟 값이 변경되면 호출
   
    [NonSerialized]
    private bool isInitialize = false;

    public void OnEnable() //스크립트에이블이 사용가능해질 때.
    {
        InitializeAttribute();
    }

    public void InitializeAttribute()
    {
        if (isInitialize)
            return;

        isInitialize = true;

        foreach (Attribute attribute in attributes)
        {
            attribute.value = new ModifiableFloat(OnModifiedValue); //스텟오브젝트에서 값이 변경된 경우로 초기화
        }

        level = 1;
        exp = 0;


        //스텟값 초기화
        SetBaseValue(Define.CharacterAttribute.HP, 100);
        SetBaseValue(Define.CharacterAttribute.Mana, 100);
        SetBaseValue(Define.CharacterAttribute.Attack, 100);
        SetBaseValue(Define.CharacterAttribute.AttackSpeed, 1f);
        SetBaseValue(Define.CharacterAttribute.Defence, 100);
        SetBaseValue(Define.CharacterAttribute.MoveSpeed, 0.1f);
        //스텟값 초기화 및 최대값으로 현재값 수정
        HP = GetModifiedValue(Define.CharacterAttribute.HP);
        Mana = GetModifiedValue(Define.CharacterAttribute.Mana);
    }

    private void OnModifiedValue(ModifiableFloat value)
    {
        OnChangeStats?.Invoke(this);
    }

    public void SetBaseValue(Define.CharacterAttribute type, float value)
    {
        foreach (Attribute attribute in attributes)
        {
            if (attribute.type == type)
                attribute.value.BaseValue = value;
        }
    }

    public float GetBaseValue(Define.CharacterAttribute type)
    {
        foreach (Attribute attribute in attributes)
        {
            if (attribute.type == type)
                return attribute.value.BaseValue;
        }
        return -1;
    }

    public float GetModifiedValue(Define.CharacterAttribute type)
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
        OnChangeStats?.Invoke(this);

        return HP;
    }

    public float AddMana(float value)
    {
        Mana += value;
        OnChangeStats?.Invoke(this);
        return Mana;
    }
}
