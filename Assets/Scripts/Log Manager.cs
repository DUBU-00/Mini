using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LogManager : MonoBehaviour
{
    public static LogManager Instance;

    [SerializeField] private Text expText;
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

    }
}
