using UnityEngine;

public class AreaBGM : MonoBehaviour
{
    [SerializeField] private AreaType areaType;

    private bool isGameStarted = false;
    
    public void SetGameStarted()
    {
        isGameStarted = true;
    }
    public enum AreaType
    {
        Village,
        Dungeon
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isGameStarted)
            return;

        if (!collision.CompareTag("Player"))
            return;
        PlayBGM();
    }

    public void PlayBGM()
    {
        switch (areaType)
        {
            case AreaType.Village:
                BGMManager.Instance.PlayVillage();
                break;

            case AreaType.Dungeon:
                BGMManager.Instance.PlayDungeon();
                break;
        }
    }
}