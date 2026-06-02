using UnityEngine;

public class BossWeapon : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

            BossAI boss = GetComponentInParent<BossAI>();

            if (playerHealth != null && boss != null)
            {
                int damage = boss.GetCurrentDamage();

                Vector2 hitDirection = (collision.transform.position - transform.position).normalized;
                hitDirection.y = 0.5f;

                playerHealth.TakeDamage(damage, hitDirection);
            }
        }
    }
}
