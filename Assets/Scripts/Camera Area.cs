using UnityEngine;

public class CameraArea : MonoBehaviour
{
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ApplyBounds(collision);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        ApplyBounds(collision);
    }
    private void ApplyBounds(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CameraFollow cam = Camera.main.GetComponent<CameraFollow>();
            cam.SetBounds(minX, maxX, minY, maxY);
            cam.MoveInstant();
        }
    }
}
