using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHealth : MonoBehaviour
{
    [SerializeField] private float knockbackPower = 5f;
    [SerializeField] private int maxHp = 3;
    [SerializeField] private Image hpFill;
    private int currentHp;
    private Rigidbody2D rb;
    private Animator anim;

    private bool isDead = false;

    void Start()
    {
        currentHp = maxHp;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(int damage, Vector2 attackDir)
    {
        if (isDead)
            return;

        currentHp -= damage;

        hpFill.fillAmount = (float)currentHp / maxHp;

        anim.SetTrigger("Hit");

        rb.linearVelocity = Vector2.zero;

        rb.AddForce(attackDir * knockbackPower,ForceMode2D.Impulse);

        StartCoroutine(HitRoutine());

        if (currentHp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        anim.SetTrigger("Die");

        StopAllCoroutines();

        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;

        GetComponent<MonsterPatrol>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        MonsterManager.Instance.MonsterDead();
        StartCoroutine(DieRoutine());
    }
    IEnumerator DieRoutine()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
    IEnumerator HitRoutine()
    {
        MonsterPatrol patrol = GetComponent<MonsterPatrol>();

        if (patrol != null)
        {
            patrol.SetHit(true);
        }

        yield return new WaitForSeconds(0.3f);
        if (isDead) yield break;

        if (patrol != null)
        {
            patrol.SetHit(false);
        }
    }
}