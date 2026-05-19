using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField]
    private Transform villageSpawn;

    [SerializeField]
    private GameObject player;

    void Start()
    {
        if (SpawnManager.spawnPointName == "VillageSpawn")
        {
            player.transform.position =
                villageSpawn.position;
        }
    }
}