using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class LogManager : MonoBehaviour
{
    public static LogManager Instance;

    [SerializeField] private TextMeshProUGUI expText;
    [SerializeField] private float fadeDuration = 1.5f;
    [SerializeField] private float moveSpeed = 30f;

    private Coroutine fadeCoroutine;
    private Vector3 originalPosition;

    private void Awake()
    {
        Instance = this;
        if (expText != null )
        {
            originalPosition = expText.transform.localPosition;
            expText.gameObject.SetActive(false);
        }
    }
    public void ShowExpLog(int amonut)
    {
        if (expText == null) 
            return;
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(CoShowLog(amonut));
    }
    private IEnumerator CoShowLog(int amonut)
    {
        expText.transform.localPosition = originalPosition;
        expText.text = $"경험치를 {amonut} 얻었습니다";

        Color textColor = expText.color;
        textColor.a = 1f;
        expText.color = textColor;

        expText.gameObject.SetActive(true);

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            expText.transform.localPosition += Vector3.up * moveSpeed * Time.deltaTime;

            textColor.a = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            expText.color = textColor;

            yield return null;
        }
        expText.gameObject.SetActive(false);
    }
}
