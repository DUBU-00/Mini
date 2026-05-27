using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private PlayerStats stats;
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Slider expSlider;
    [SerializeField] private TextMeshProUGUI levelText;

    private void Update()
    {
        UpdateUI();
    }
    private void UpdateUI()
    {
        hpSlider.maxValue = stats.maxHp;
        hpSlider.value = stats.currentHp;
        expSlider.maxValue = stats.maxExp;
        expSlider.value = stats.currentExp;

        levelText.text = "Lv." + stats.level;
    }
}
