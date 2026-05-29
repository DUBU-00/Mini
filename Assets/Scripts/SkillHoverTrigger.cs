using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillHoverTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private PlayerStats playerStats;
    private Sprite myIconSprite;

    private void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null) playerStats = player.GetComponent<PlayerStats>();
        if (playerStats == null) playerStats = FindFirstObjectByType<PlayerStats>();

        Image img = GetComponent<Image>();
        if (img != null) myIconSprite = img.sprite;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (playerStats == null || SkillTooltip.Instance == null)
            return;

        string sName = "파이어볼";
        int currentLvl = playerStats.fireballLevel;
        int maxLvl = PlayerStats.MAX_FIREBALL_LEVEL;

        string description = "전방으로 파이어볼을 발사한다. 레벨이 증가 할수록 기본 공격력에 비례해 파이어볼 데미지도 증가한다.\n" +
                             "<color=#FFFF55>(마스터 레벨 달성 시 기본 공격력의 최대 150% 피해)</color>";
        string effects = "";

        if (currentLvl > 0)
        {
            float currentRatio = Mathf.Lerp(0.5f, 1.5f, (float)(currentLvl - 1) / 19f);
            int currentDamage = Mathf.FloorToInt(playerStats.NormalAttack * currentRatio);
            effects += $"<color=#FFFFFF>[현재레벨 {currentLvl}]</color>\n" +
                       $"파이어볼 데미지 {currentDamage}\n" + 
                       $"기본 공격력의 {GetPercent(currentLvl)}% 상승";
        }

        if (currentLvl < maxLvl)
        {
            if (currentLvl > 0) effects += "\n\n";

            int nextLvl = currentLvl + 1;
            float nextRatio = Mathf.Lerp(0.5f, 1.5f, (float)(nextLvl - 1) / 19f);
            int nextDamage = Mathf.FloorToInt(playerStats.NormalAttack * nextRatio);

            effects += $"<color=#F5A623>[다음레벨 {nextLvl}]</color>\n" +
                       $"파이어볼 데미지 {nextDamage}\n" +
                       $"기본 공격력의 {Mathf.RoundToInt(nextRatio * 100f)}% 상승";
        }
        SkillTooltip.Instance.showTooltip(myIconSprite, sName, currentLvl, maxLvl, description, effects);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (SkillTooltip.Instance != null)
        {
            SkillTooltip.Instance.HideTooltip();
        }
    }
    private int GetPercent(int level)
    {
        float ratio = Mathf.Lerp(0.5f, 1.5f, (float)(level - 1) / 19f);
        return Mathf.RoundToInt(ratio * 100f);
    }
}
