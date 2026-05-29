using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillTooltip : MonoBehaviour
{
    public static SkillTooltip Instance;

    [SerializeField] private Image skillIcon;
    [SerializeField] private TextMeshProUGUI skillNameText;
    [SerializeField] private TextMeshProUGUI masterLvlText;
    [SerializeField] private TextMeshProUGUI descText;
    [SerializeField] private TextMeshProUGUI effectsText;

    [SerializeField] private Vector2 offset = new Vector2(15f, -15f);

    private RectTransform rectTransform;

    private void Awake()
    {
        Instance = this;
        rectTransform = GetComponent<RectTransform>();
        gameObject.SetActive(false);
    }
    private void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        rectTransform.position = mousePos + offset;
    }
    public void showTooltip(Sprite icon, string sName, int currentLvl, int maxLvl, string description, string effects)
    {
        gameObject.SetActive(true);
        if (skillIcon != null && icon != null) skillIcon.sprite = icon;

        skillNameText.text = sName;
        masterLvlText.text = $"[마스터 레벨 : {maxLvl}]";
        descText.text = description;
        effectsText.text = effects;
    }
    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}
