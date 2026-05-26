using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LodingPopupUI : MonoBehaviour
{
    [SerializeField] private RawImage RawImage_Loadingimage;
    [SerializeField] private Slider Slider_Loadingbar;

    private Coroutine loadingCoroutine;
    private GameObject hpUI;

    private void OnEnable()
    {
        LoadImage();
        if (loadingCoroutine != null )
        {
            StopCoroutine(loadingCoroutine);
        }
        loadingCoroutine = StartCoroutine(CoStartLoadingbar());
    }
    private void LoadImage()
    {
        var texture = Resources.Load<Texture>("Texture/Game_Loading");
        if (texture != null )
        {
            RawImage_Loadingimage.texture = texture;
        }
    }
    IEnumerator CoStartLoadingbar()
    {
        Slider_Loadingbar.value = 0f;
        yield return StartCoroutine(SetLoading(0.1f, 0.15f));
        yield return StartCoroutine(SetLoading(0.5f, 0.55f));
        yield return StartCoroutine(SetLoading(0.75f, 1.0f));
        yield return StartCoroutine(SetLoading(1.0f, 1.2f));

        this.gameObject.SetActive(false);
        BGMManager.Instance.PlayVillage();
    }
    private IEnumerator SetLoading(float taeget, float duration)
    {
        float start = Slider_Loadingbar.value;
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            Slider_Loadingbar.value = Mathf.Lerp(start, taeget, time /  duration);

            yield return null;
        }
        Slider_Loadingbar.value = taeget;
    }
}
