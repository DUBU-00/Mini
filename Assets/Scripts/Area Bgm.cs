using UnityEngine;

public class AreaBGM : MonoBehaviour
{
    public enum AreaType
    {
        Village,
        Dungeon
    }

    [SerializeField]
    private AreaType areaType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
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