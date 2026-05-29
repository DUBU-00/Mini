using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LodingPopupUI : MonoBehaviour
{
    [SerializeField] private RawImage RawImage_Loadingimage;
    [SerializeField] private Slider Slider_Loadingbar;
    [SerializeField] private GameObject gameStartButton;

    private Coroutine loadingCoroutine;

    private void OnEnable()
    {
        LoadImage();
        if (loadingCoroutine != null)
        {
            StopCoroutine(loadingCoroutine);
        }
        loadingCoroutine = StartCoroutine(CoStartLoadingbar());
    }

    private void LoadImage()
    {
        var texture = Resources.Load<Texture>("Texture/Game_Loading");
        if (texture != null)
        {
            RawImage_Loadingimage.texture = texture;
        }
    }

    IEnumerator CoStartLoadingbar()
    {
        if (Slider_Loadingbar == null)
        {
            Debug.LogError("LodingPopupUI: Slider_Loadingbar가 인스펙터에서 할당되지 않았습니다!");
            yield break;
        }

        Slider_Loadingbar.value = 0f;
        yield return SetLoading(0.1f, 0.15f);
        yield return SetLoading(0.5f, 0.55f);
        yield return SetLoading(0.75f, 1.0f);
        yield return SetLoading(1.0f, 1.2f);
        Slider_Loadingbar.gameObject.SetActive(false);

        if (gameStartButton != null)
            gameStartButton.SetActive(true);

        if (GameManager.Instance == null || GameManager.Instance.playerStats == null)
        {
            Debug.LogWarning("GameManager 또는 playerStats가 아직 준비되지 않아 BGM을 재생할 수 없습니다.");
            yield break;
        }

        PlayerStats player = GameManager.Instance.playerStats;
        Collider2D area = Physics2D.OverlapPoint(player.transform.position);

        if (area != null)
        {
            AreaBGM areaBgm = area.GetComponent<AreaBGM>();
            if (areaBgm != null)
            {
                areaBgm.PlayBGM();
            }
        }
    }

    private IEnumerator SetLoading(float taeget, float time)
    {
        float start = Slider_Loadingbar.value;
        float t = 0f;
        while (t < time)
        {
            t += Time.deltaTime;
            Slider_Loadingbar.value = Mathf.Lerp(start, taeget, t / time);
            yield return null;
        }
        Slider_Loadingbar.value = taeget;
    }

    public void OnClickGameStart()
    {
        StartCoroutine(CoGameStartSequence());
    }

    private IEnumerator CoGameStartSequence()
    {
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.ApplyLoadedGame();
        }

        AreaBGM.SetGameStarted();

        if (GameManager.Instance == null || GameManager.Instance.playerStats == null)
            yield break;

        PlayerStats player = GameManager.Instance.playerStats;

        SaveData data = SaveManager.loadedData;
        Vector3 targetPosition = new Vector3(data.posX, data.posY, 0);

        player.transform.position = targetPosition;
        Physics2D.SyncTransforms();

        yield return new WaitForEndOfFrame();

        if (Camera.main != null && Camera.main.TryGetComponent<CameraFollow>(out var cameraFollow))
        {
            cameraFollow.MoveInstant();
        }

        Collider2D[] areas = Physics2D.OverlapPointAll(targetPosition);
        foreach (Collider2D area in areas)
        {
            AreaBGM bgm = area.GetComponent<AreaBGM>();
            if (bgm != null)
            {
                bgm.PlayBGM();
                break;
            }
        }

        if (gameStartButton != null) gameStartButton.SetActive(false);
        this.gameObject.SetActive(false);
    }
}