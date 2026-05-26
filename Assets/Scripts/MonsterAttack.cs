using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth player = collision.GetComponent<PlayerHealth>();

            if (player != null)
            {
                Vector2 dir = (collision.transform.position - transform.position).normalized;

                player.TakeDamage(damage, dir);
            }
        }
    }
}