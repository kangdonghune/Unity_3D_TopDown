using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInGameUI : MonoBehaviour
{
    [HideInInspector]
    public StatsObject playerStats;

    public TextMeshProUGUI levelText;
    public Slider HpSlider;
    public TextMeshProUGUI HPText;
    public Slider ManaSlider;
    public TextMeshProUGUI ManaText;

    void Start()
    {
        levelText.text = playerStats.level.ToString();
        HpSlider.value = playerStats.HealthPercentage;
        ManaSlider.value = playerStats.ManaPercentage;
        HPText.text = playerStats.HP.ToString("n0");
        ManaText.text = playerStats.Mana.ToString("n0");
    }

    public void AddEvent()
    {
        playerStats.OnChangeStats -= OnChangedStats;
        playerStats.OnChangeStats += OnChangedStats;
    }

    private void OnDisable()
    {
        playerStats.OnChangeStats -= OnChangedStats;
    }

    private void OnChangedStats(StatsObject statsObject)
    {
        levelText.text = statsObject.level.ToString();
        HpSlider.value = statsObject.HealthPercentage;
        ManaSlider.value = statsObject.ManaPercentage;
        HPText.text = statsObject.HP.ToString("n0");
        ManaText.text = statsObject.Mana.ToString("n0");
    }
}
