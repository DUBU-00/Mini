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
                if (BGMManager.Instance != null) BGMManager.Instance.PlayVillage();
                break;

            case AreaType.Dungeon:
                if (BGMManager.Instance != null) BGMManager.Instance.PlayDungeon();
                break;
        }
    }
}