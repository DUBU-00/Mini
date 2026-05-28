using TMPro;
using UnityEngine;

public class PotionUI : MonoBehaviour
{
    [SerializeField] private PlayerStats stats;
    [SerializeField] private TextMeshProUGUI potionText;

    private void Update()
    {
        potionText.text = "x" + stats.potionCount;
    }
}
