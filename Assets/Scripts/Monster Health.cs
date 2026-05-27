using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHealth : MonoBehaviour
{
    [SerializeField] private float knockbackPower = 5f;
    [SerializeField] private int maxHp = 3;
    [SerializeField] private Image hpFill;
    [SerializeField] private float respawnTime = 7f;
    [SerializeField] private GameObject hpBarobject;
    [SerializeField] private Collider2D bodyCollider;
    [SerializeField] private Collider2D attackCollider;
    [SerializeField] private BoxCollider2D attackTrigger;
    [SerializeField] private int expReward = 20;

    private int currentHp;
    private Rigidbody2D rb;
    private Animator anim;
    private MonsterPatrol patrol;
    private bool isDead = false;
    private Vector3 startPos;
    private Collider2D col;
    private SpriteRenderer sr;

    void Start()
    {
        currentHp = maxHp;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        patrol = GetComponent<MonsterPatrol>();
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        startPos = transform.position;
    }

    public void TakeDamage(int damage, Vector2 attackDir)
    {
        if (isDead)
            return;

        currentHp -= damage;

        hpFill.fillAmount = (float)currentHp / maxHp;

        if (patrol != null)
        {
            patrol.StartChase();
        }

        rb.linearVelocity = Vector2.zero;

        rb.AddForce(attackDir * knockbackPower,ForceMode2D.Impulse);

        if (currentHp <= 0)
        {
            Die();
            return;
        }
        StartCoroutine(HitRoutine());
        anim.SetTrigger("Hit");
    }


    void Die()
    {
        anim.ResetTrigger("Hit");
        isDead = true;
        anim.SetTrigger("Die");
        attackTrigger.enabled = false;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;

        patrol.enabled = false;
        col.enabled = false;

        hpBarobject.SetActive(false);
        GameManager.Instance.playerStats.AddExp(expReward);

        StartCoroutine(RespawnRoutine());
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
    IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(1f);
        bodyCollider.enabled = false;
        rb.bodyType = RigidbodyType2D.Kinematic;

        col.enabled = false;

        hpBarobject.SetActive(false);
        sr.enabled = false;

        yield return new WaitForSeconds(respawnTime);

        transform.position = startPos;
        attackCollider.enabled = true;
        bodyCollider.enabled = true;
        currentHp = maxHp;
        hpFill.fillAmount = 1;

        isDead = false;

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.linearVelocity = Vector2.zero;

        col.enabled = true;

        patrol.enabled = true;
        patrol.ResetState();

        hpBarobject.SetActive(true);

        sr.enabled = true;

        anim.Rebind();
        anim.Update(0f);
        anim.Play("Idle", 0, 0f);
    }
}