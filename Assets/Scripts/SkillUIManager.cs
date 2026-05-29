using UnityEngine;

public class SkillUIManager : MonoBehaviour
{
    [SerializeField] private GameObject skillUIPanel;

    void Start()
    {
        if (skillUIPanel == null)
        {
            skillUIPanel.SetActive(false);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            ToggleSkillUI();
        }
    }
    public void ToggleSkillUI()
    {
        if (skillUIPanel == null)
            return;

        bool isActive = skillUIPanel.activeSelf;
        skillUIPanel.SetActive(!isActive);
    }
}
