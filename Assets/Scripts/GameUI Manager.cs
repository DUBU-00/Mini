using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private GameObject hpUI;

    private void Start()
    {
        hpUI.SetActive(false);
    }

    public void ShowGameUI()
    {
        hpUI.SetActive(true);
    }
}