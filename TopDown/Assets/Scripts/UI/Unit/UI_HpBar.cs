using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HpBar : UI_Unit
{
    enum Sliders
    {
        HP,
    }

    private Slider _hpSlider;

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


}
