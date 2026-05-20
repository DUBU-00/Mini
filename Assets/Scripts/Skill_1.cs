using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillFireball : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private int damage = 1;
    [SerializeField] SpriteRenderer SpriteRenderer_Effect;

    void Start()
    {
        Destroy(gameObject, 2f);
    }
    private Vector3 _moveDirection = new Vector3(1, 0, 0);
    public void InitSkill(bool isDirRight, Vector3 playerpos)
    {
        this.transform.position = playerpos;

        // 사이드뷰 기준 x값만 좌 우 1 또는 -1로 지정됨
        _moveDirection = isDirRight ? new Vector3(1, 0, 0) : new Vector3(-1, 0, 0);
        SpriteRenderer_Effect.flipX = !isDirRight;
        SpriteRenderer_Effect.flipY = !isDirRight;
    }
    void Update()
    {
        //int a = 1;
        //transform.Translate(new Vector3(((speed * Time.deltaTime) * a),0,0));
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
