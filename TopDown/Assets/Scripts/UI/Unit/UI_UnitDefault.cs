using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitDefault : UI_Unit
{
    enum Sliders
    {
        HP,
    }

    public Slider _hpSlider;
    public GameObject DamageText;
    

    public float MinimumValue
    {
        get => _hpSlider.minValue;
        set { _hpSlider.minValue = value; }
    }

    public float MaximumValue
    {
        get => _hpSlider.maxValue;
        set { _hpSlider.maxValue = value; }
    }

    public float Value
    {
        get => _hpSlider.value;
        set { _hpSlider.value = value; }
    }

    public override void Init()
    {
        base.Init();
        Bind<Slider>(typeof(Sliders));
        _hpSlider = Get<Slider>((int)Sliders.HP);

    }

    public void CreateDamageText(int damage)
    {
        GameObject damageText;

        if (DamageText == null)
        {
            damageText = Managers.Resource.Instantiate("UI/Unit/DamageText", transform);
            damageText.GetOrAddComponent<CameraFacing>();
            damageText.GetOrAddComponent<TextDuration>();
        }

        else
        {
            damageText = Object.Instantiate(DamageText, transform);
            damageText.GetOrAddComponent<CameraFacing>();
            damageText.GetOrAddComponent<TextDuration>();
        }

        TextMeshProUGUI TMP = damageText.GetComponent<TextMeshProUGUI>();
        TMP.text = damage.ToString();

    }

}
