using System.Collections;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LogManager : MonoBehaviour
{
    public static LogManager Instance;

    [SerializeField] private GameObject expTextPrefab;
    [SerializeField] private Transform logContainer;
    [SerializeField] private float fadeDuration = 1.5f;
    [SerializeField] private float moveSpeed = 60f;

    private float lastSpawnTime;
    private float currentSpawnOffset = 0f;

    private void Awake()
    {
        Instance = this;

        if (logContainer != null)
        {
            RectTransform containerRect = logContainer.GetComponent<RectTransform>();
            if (containerRect != null)
            {
                containerRect.anchorMin = new Vector2(1, 0);
                containerRect.anchorMax = new Vector2(1, 0);
                containerRect.pivot = new Vector2(1, 0);
                containerRect.anchoredPosition = Vector2.zero;
            }
        }
    }
    public void ShowExpLog(int amonut)
    {
        if (expTextPrefab == null || logContainer == null) 
            return;
        if (Time.time - lastSpawnTime < 0.25f)
        {
            currentSpawnOffset += 35f;
        }
        else
        {
            currentSpawnOffset = 0f;
        }
        lastSpawnTime = Time.time;

        GameObject newLog = Instantiate(expTextPrefab, logContainer);
        newLog.transform.localScale = Vector3.one;
        newLog.SetActive(true);

        StartCoroutine(CoFadeOutLog(newLog, amonut, currentSpawnOffset));
    }
    private IEnumerator CoFadeOutLog(GameObject logObj, int amonut, float startYOffset)
    {
        RectTransform rectTransform = logObj.GetComponent<RectTransform>();
        TextMeshProUGUI textPro = logObj.GetComponent<TextMeshProUGUI>();

        if (textPro != null)
        {
            textPro.text = $"경험치를 ({amonut}) 얻었습니다";
            Color textColor = textPro.color;

            rectTransform.anchorMin = new Vector2(1, 0);
            rectTransform.anchorMax = new Vector2(1, 0);
            rectTransform.pivot = new Vector2(1, 0);

            rectTransform.anchoredPosition = new Vector2(-30, 100 + startYOffset);
            float timer = 0f;
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;

                rectTransform.anchoredPosition += Vector2.up * moveSpeed * Time.deltaTime;

                textColor.a = Mathf.Lerp(1f, 0f, timer / fadeDuration);
                textPro.color = textColor;

                yield return null;
            }
        }
        Destroy(logObj);
    }
}
