using TMPro;
using UnityEngine;

public class PotionUI : MonoBehaviour
{
    [SerializeField] private PlayerStats stats;
    [SerializeField] private TextMeshProUGUI hpPotionText;
    [SerializeField] private TextMeshProUGUI mpPotionText;

    private void Update()
    {
       if (stats != null)
       {
           if (hpPotionText != null)
               hpPotionText.text = "x" + stats.hpPotionCount;
           if (mpPotionText != null)
               mpPotionText.text = "x" + stats.mpPotionCount;
       }
    }
}
