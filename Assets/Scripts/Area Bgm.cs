using UnityEngine;

public class AreaBGM : MonoBehaviour
{
    [SerializeField] private AreaType areaType;

    private static bool isGameStarted = false;
    
    public static void SetGameStarted()
    {
        isGameStarted = true;
    }
    public enum AreaType
    {
        Village,
        Dungeon,
        Dungeon_3,
        Boss
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
                if (BGMManager.Instance != null) BGMManager.Instance.PlayVillage();
                break;

            case AreaType.Dungeon:
                if (BGMManager.Instance != null) BGMManager.Instance.PlayDungeon();
                break;

            case AreaType.Dungeon_3:
                if (BGMManager.Instance != null) BGMManager.Instance.PlayDungeon_3();
                break;

            case AreaType.Boss:
                if (BGMManager.Instance != null) BGMManager.Instance.PlayBoss();
                break;
        }
    }
}