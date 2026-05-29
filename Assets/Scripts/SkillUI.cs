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
        if (player != null )
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
        EnsurePlayerStatsConnected();
        UpdateSkillUI();
    }
    private void EnsurePlayerStatsConnected()
    {
        if (stats == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                stats = player.GetComponent<PlayerStats>();
            }
            else
            {
                stats = FindFirstObjectByType<PlayerStats>();
            }
        }
    }
    public void UpdateSkillUI()
    {
        if (stats == null)
            return;
        if (skillPointText != null)
            skillPointText.text = "SP : " + stats.skillPoints;
        if (fireballLvText != null)
        {
            if (stats.fireballLevel >= PlayerStats.MAX_FIREBALL_LEVEL)
                fireballLvText.text = "파이어볼 Lv.20 (MAX)";
            else
                fireballLvText.text = "파이어볼 Lv." + stats.fireballLevel;
        }
        if (upgradeButton != null)
        {
            if (stats.skillPoints > 0 && stats.fireballLevel < PlayerStats.MAX_FIREBALL_LEVEL)
                upgradeButton.SetActive(true);
            else
                upgradeButton.SetActive(false);
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
