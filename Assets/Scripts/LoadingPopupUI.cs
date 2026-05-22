using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LodingPopupUI : MonoBehaviour
{
    [SerializeField] private RawImage RawImage_Loadingimage;
    [SerializeField] private Slider Slider_Loadingbar;

    private void OnEnable()
    {
        LoadandImage();
        StartCoroutine(CoStartLoadingbar());
    }
    private void LoadandImage()
    {
        var texture = Resources.Load<Texture>("Texture/Game_Loading");
        if (texture != null )
        {
            RawImage_Loadingimage.texture = texture;
        }
    }
    IEnumerator CoStartLoadingbar()
    {
        Slider_Loadingbar.value = 0.2f;
        yield return new WaitForSeconds(0.3f);
        Slider_Loadingbar.value = 0.4f;
        yield return new WaitForSeconds(0.5f);
        Slider_Loadingbar.value = 0.75f;
        yield return new WaitForSeconds(1.0f);
        Slider_Loadingbar.value = 1.0f;
        yield return new WaitForSeconds(1.2f);

        this.gameObject.SetActive(false);
    }
}
