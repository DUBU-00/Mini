using TMPro;
using UnityEngine;

public class StatUI : MonoBehaviour
{
    [SerializeField] private GameObject statPanel;
    [SerializeField] private PlayerStats stats;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI attack1Text;
    [SerializeField] private TextMeshProUGUI attack2Text;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI expText;

    private bool isOpen = false;

    private void Start()
    {
        statPanel.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            ToggleStatPanel();
        }
        if (isOpen)
        {
            UpdateUI();
        }
    }
    private void ToggleStatPanel()
    {
        isOpen = !isOpen;
        statPanel.SetActive(isOpen);
    }
    private void UpdateUI()
    {
        levelText.text = "Lv." + stats.level;
        hpText.text = "HP : " + stats.currentHp + " / " + stats.maxHp;
        attack1Text.text = "N.ATK : " + stats.NormalAttack;
        attack2Text.text = "H.ATK : " + stats.HardAttack;
        speedText.text = "SPD : " + stats.moveSpeed.ToString("F1");
        expText.text = "EXP : " + stats.currentExp + " / " + stats.maxExp;
    }
}
