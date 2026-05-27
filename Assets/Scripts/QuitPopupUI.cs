using UnityEditor;
using UnityEngine;

public class QuitPopupUI : MonoBehaviour
{
    [SerializeField] private GameObject quitPopup;
    private bool isOpen = false;

    private void Start()
    {
        quitPopup.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            QuitPopup();
        }
    }
    private void QuitPopup()
    {
        isOpen = !isOpen;
        quitPopup.SetActive(isOpen);
        Time.timeScale = isOpen ? 0f : 1f;
    }
    public void OnClickYes()
    {
        SaveManager.Instance.SaveGame();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public void OnClickNo()
    {
        isOpen = false;
        quitPopup.SetActive(false);
        Time.timeScale = 1f;
    }
}