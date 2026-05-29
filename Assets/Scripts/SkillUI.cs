using TMPro;
using UnityEngine;

public class SkillUI : MonoBehaviour
{
    private PlayerStats stats;

    [SerializeField] private TextMeshProUGUI skillPointText;
    [SerializeField] private TextMeshProUGUI fireballLvText;
    [SerializeField] private GameObject upgradeButton;

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null )
        {
            stats = player.GetComponent<PlayerStats>();
        }
        else
        {
            stats = FindFirstObjectByType<PlayerStats>();
        }
    }
    void OnEnable()
    {
        UpdateSkillUI();
    }
    public void UpdateSkillUI()
    {
        if (stats == null)
            return;
        if (skillPointText != null)
            skillPointText.text = "SP : " + stats.skillPoints;
        if (fireballLvText != null)
            fireballLvText.text = "파이어볼 Lv." + stats.fireballLevel;
        if (upgradeButton != null)
        {
            if (stats.skillPoints > 0)
            {
                upgradeButton.SetActive(true);
            }
            else
            {
                upgradeButton.SetActive(false);
            }
        }
    }
    public void OnClickUpgradeFireball()
    {
        if (stats != null && stats.UpgradeFireball())
        {
            UpdateSkillUI();
        }
    }
}
