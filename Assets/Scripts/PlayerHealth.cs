using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHp = 5;
    [SerializeField] private Image hpFill;

    [SerializeField] private float knockbackPower = 5f;
    [SerializeField] private float invincibleTime = 1f;

    private int currentHp;
    private bool isInvincible = false;
    private bool isHit = false;

    private Rigidbody2D rb;
    private Animator anim;

    void Start()
    {
        currentHp = maxHp;

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        UpdateHpUI();
    }

    private void Update()
    {
        hpFill.fillAmount = Mathf.Lerp(hpFill.fillAmount,(float)currentHp / maxHp,Time.deltaTime * 5f);
    }
    public void TakeDamage(int damage, Vector2 hitDirection)
    {
        if (isInvincible || isHit) 
            return;

        isHit = true;
        currentHp -= damage;

        if (currentHp < 0)
            currentHp = 0;

        UpdateHpUI();

        anim.SetTrigger("Hit");

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(hitDirection * knockbackPower, ForceMode2D.Impulse);

        StartCoroutine(CoInvincible());
        StartCoroutine(CoHit());

        if (currentHp <= 0)
        {
            Die();
        }
    }

    private IEnumerator CoInvincible()
    {
        isInvincible = true;

        yield return new WaitForSeconds(invincibleTime);

        isInvincible = false;
    }

    private void UpdateHpUI()
    {
        hpFill.fillAmount = (float)currentHp / maxHp;
    }

    private void Die()
    {
        anim.SetTrigger("Die");
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
}