using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager Instance;

    private int monsterCount;

    //[SerializeField]
    //private GameObject clearPopup;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        MonsterHealth[] monsters = FindObjectsByType<MonsterHealth>(FindObjectsSortMode.None);
        monsterCount = monsters.Length;
        //clearPopup.SetActive(false);
    }

    public void MonsterDead()
    {
        monsterCount--;
        Debug.Log(monsterCount);
        if (monsterCount <= 0)
        {
            //StageClear();
        }
    }

    //void StageClear()
    //{
        //clearPopup.SetActive(true);
        //Time.timeScale = 0f;
    //}
}