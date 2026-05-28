using System.Collections;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Transform destination;
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;
    [SerializeField] private bool isVillagePortal;

    private bool canTeleport = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canTeleport)
            return;

        if (!collision.CompareTag("Player"))
            return;

        canTeleport = false;

        collision.transform.position = destination.position;

        if(isVillagePortal)
        {
            GameManager.Instance.playerStats.FullRecovery();
        }

        CameraFollow cam = Camera.main.GetComponent<CameraFollow>();

        cam.SetBounds(minX, maxX, minY, maxY);

        cam.MoveInstant();

        StartCoroutine(TeleportCooldown());
    }

    IEnumerator TeleportCooldown()
    {
        yield return new WaitForSeconds(1f);

        canTeleport = true;
    }
}