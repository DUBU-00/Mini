using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private Image hpFill;

    [SerializeField] private float knockbackPower = 5f;
    [SerializeField] private float invincibleTime = 1f;

    private bool isInvincible = false;
    private bool isHit = false;
    private bool isDie = false;

    private Rigidbody2D rb;
    private Animator anim;
    private PlayerStats stats;

    void Start()
    {
        stats = GetComponent<PlayerStats>();

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        UpdateHpUI();
    }

    private void Update()
    {
        hpFill.fillAmount = Mathf.Lerp(hpFill.fillAmount,(float)stats.currentHp / stats.maxHp,Time.deltaTime * 5f);
    }
    public void TakeDamage(int damage, Vector2 hitDirection)
    {
        if (isDie || isInvincible || isHit) 
            return;

        isHit = true;
        stats.currentHp -= damage;

        if (stats.currentHp < 0)
            stats.currentHp = 0;

        UpdateHpUI();

        if (stats.currentHp <= 0)
        {
            Die();
            return;
        }
        isHit = true;
        anim.SetTrigger("Hit");

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(hitDirection * knockbackPower, ForceMode2D.Impulse);

        StartCoroutine(CoInvincible());
        StartCoroutine(CoHit());
    }

    private IEnumerator CoInvincible()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }

    private void UpdateHpUI()
    {
        hpFill.fillAmount = (float)stats.currentHp / stats.maxHp;
    }

    private void Die()
    {
        isDie = true;
        anim.SetTrigger("Die");
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;
    }
    private IEnumerator CoHit()
    {
        yield return new WaitForSeconds(0.3f);
        isHit = false;
    }
    public bool IsHit()
    {
        return isHit;
    }
    public bool IsDie()
    {
        return isDie;
    }
}