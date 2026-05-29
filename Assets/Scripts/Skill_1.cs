using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillFireball : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] SpriteRenderer SpriteRenderer_Effect;
    private int damage;

    private Vector3 _moveDirection = new Vector3(1, 0, 0);

    void Start()
    {
        Destroy(gameObject, 2f);
    }
    public void InitSkill(bool isDirRight, Vector3 playerpos, PlayerStats stats)
    {
        this.transform.position = playerpos;

        if (stats != null)
        {
            damage = stats.FireballDamage;
        }
        
        _moveDirection = isDirRight ? new Vector3(1, 0, 0) : new Vector3(-1, 0, 0);
        SpriteRenderer_Effect.flipX = !isDirRight;
        SpriteRenderer_Effect.flipY = !isDirRight;
    }
    void Update()
    {
        transform.position += _moveDirection * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            MonsterHealth hp = other.GetComponent<MonsterHealth>();
            if(hp != null)
            {
                Vector2 attackDir = (other.transform.position - transform.position).normalized;
                hp.TakeDamage(damage, attackDir);
            }
            Destroy(gameObject);
        }
    }
}
