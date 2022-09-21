using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatUI : MonoBehaviour
{
    [HideInInspector]
    public InventoryObject equipment;
    [HideInInspector]
    public StatsObject playerStats;

    public TextMeshProUGUI[] attribute;

    private void OnEnable()
    {
        playerStats.OnChangeStats += OnChangedStats;

        if(equipment != null && playerStats != null)
        {
            foreach(InventorySlot slot in equipment.slots)
            {
                slot.OnPreUpdate -= OnRemoveItem;
                slot.OnPostUpdate -= OnEquipItem;
                slot.OnPreUpdate += OnRemoveItem;
                slot.OnPostUpdate += OnEquipItem;
            }
        }

        UpdateAttributeTexts();

    }

    private void OnDisable()
    {
        playerStats.OnChangeStats -= OnChangedStats;

        foreach (InventorySlot slot in equipment.slots)
        {
            slot.OnPreUpdate -= OnRemoveItem;
            slot.OnPostUpdate -= OnEquipItem;
        }
    }

    private void UpdateAttributeTexts()
    {
        attribute[0].text = "ATK:" + playerStats.GetModifiedValue(Define.CharacterAttribute.Attack).ToString("n0");
        attribute[1].text = "DEF:" + playerStats.GetModifiedValue(Define.CharacterAttribute.Defence).ToString("n0");
        attribute[2].text = "ATK-Speed:" + playerStats.GetModifiedValue(Define.CharacterAttribute.AttackSpeed).ToString("n2");
        attribute[3].text = "Move-Speed:" + (playerStats.GetModifiedValue(Define.CharacterAttribute.MoveSpeed)*10f).ToString("n2");
    }

    private void OnRemoveItem(InventorySlot slot)
    {
        if (slot.ItemObject == null)
            return;

        foreach (ItemBuff buff in slot.item.buffs)
        {
            foreach(Attribute attribute in playerStats.attributes)
            {
                if(attribute.type == buff.stat)
                {
                    attribute.value.RemoveModifier(buff);
                }
            }
        }
    }

    private void OnEquipItem(InventorySlot slot)
    {
        if (slot.ItemObject == null)
            return;

        foreach (ItemBuff buff in slot.item.buffs)
        {
            foreach (Attribute attribute in playerStats.attributes)
            {
                if (attribute.type == buff.stat)
                {
                    attribute.value.AddModifier(buff);
                }
            }
        }
    }

    private void OnChangedStats(StatsObject statsObject)
    {
        UpdateAttributeTexts();
    }

    public void SetRendering(bool isRendering)
    {
        Image image = gameObject.GetComponent<Image>();
        image.enabled = isRendering;
        foreach (TextMeshProUGUI text in attribute)
        {
            text.enabled = isRendering;
        }
    }
}
