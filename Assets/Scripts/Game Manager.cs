using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public PlayerStats playerStats;

    private void Awake()
    {
        Instance = this;
    }
}
