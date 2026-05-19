using UnityEngine;
using UnityEngine.UIElements;

public class Skill : MonoBehaviour
{
    [SerializeField] private Transform skillPoint;
    [SerializeField] private float skillRange = 3f;
    [SerializeField] private int skillDamage = 6;
    [SerializeField] private LayerMask monsterLayer;

    private Animator _animator;
    void Start()
    {
        _animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            UseSkill();
        }
    }
    void UseSkill()
    {
        _animator.SetTrigger("Skill");
    }
    public void SkillHit()
    {
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(skillPoint.position, skillRange, monsterLayer);

        foreach (Collider2D col in hitObjects)
        {
            MonsterHealth monster = col.GetComponent<MonsterHealth>();

            if (monster != null)
            {
                Vector2 Dir = (monster.transform.position - transform.position).normalized;
                monster.TakeDamage(skillDamage, Dir * 1f);
            }
        }
    }
}
