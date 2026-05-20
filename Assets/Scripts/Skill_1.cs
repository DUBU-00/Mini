using UnityEngine;

public class SkillFireball : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private int damage = 1;

    void Start()
    {
        Destroy(gameObject, 2f);
    }
    void Update()
    {
        transform.Translate(transform.right * speed * Time.deltaTime);
    
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            MonsterHealth hp = other.GetComponent<MonsterHealth>();
            if(hp != null)
            {
                Vector2 attackDir = transform.right;
                hp.TakeDamage(damage, attackDir);
            }
            Destroy(gameObject);
        }
    }
}
