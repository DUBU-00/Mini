using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Spine.Unity;

public class BossAI : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    public Transform player;

    [Header("보스 체력 및 UI 시스템")]
    [SerializeField] private int maxHp = 10000;
    [SerializeField] private Image hpFill;
    [SerializeField] private GameObject hpBarObject;
    [SerializeField] private float knockbackPower = 2f;
    [SerializeField] private int expReward = 500;
    [SerializeField] private AudioClip hitSfx;
    [SerializeField] private AudioClip dieSfx;

    [Header("컴포넌트 및 콜라이더 제어")]
    [SerializeField] private Collider2D bodyCollider;
    [SerializeField] private Collider2D attackCollider;
    [SerializeField] private BoxCollider2D attackTrigger;

    [Header("보스 AI 거리 및 속도 설정")]
    public float walkSpeed = 3f;
    public float runSpeed = 4.5f;
    public float detectRange = 10f;
    public float attackRange = 5f;
    public float skillRange = 6f;
    public float loseSightRange = 30f;

    [Header("공격별 데미지 설정")]
    public int damageAttack = 30;
    public int damageAttack2 = 50;
    public int damageSkill = 80;
    public int damageGap = 25;

    [Header("타격 타이밍 (초 단위 딜레이)")]
    public float delayAttack = 0.5f;
    public float delayAttack2 = 0.7f;
    public float delaySkill = 1.0f;
    public float delayGap = 0.6f;

    [Header("타격 유지 시간 (히트박스 켜짐 유지)")]
    public float durationAttack = 0.2f;
    public float durationAttack2 = 0.3f;
    public float durationSkill = 1.5f;
    public float durationGap = 0.4f;

    [Header("스파인 애니메이션 이름")]
    public string animIdle = "Idle";
    public string animAttack = "Attack";
    public string animAttack2 = "Attack2";
    public string animRun = "Run";
    public string animRun2 = "Run2";
    public string animDamage = "Damage taken";
    public string animDeath = "Death";
    public string animSkill = "skill";
    public string animGap = "gap";

    public enum State { Idle, MoveToPlayer, SkillAttack, ComboAttack, Hit, Dead }
    private State currentState = State.Idle;

    private int currentHp;
    private string currentAnim = "";
    private bool isActionPlaying = false;
    private float stateTimer = 0f;
    private int currentAttackDamage;
    private bool isUIActivated = false;
    private Vector3 startPos;

    private Rigidbody2D rb;
    private AudioSource audioSource;

    void Start()
    {
        currentHp = maxHp;
        if (hpFill != null) hpFill.fillAmount = 1f;

        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        if (skeletonAnimation == null) skeletonAnimation = GetComponent<SkeletonAnimation>();

        PlayAnim(animIdle, true);
    }

    void Update()
    {
        if (currentState == State.Dead) return;
        if (player == null) return;
        if (isActionPlaying) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > loseSightRange && isUIActivated)
        {
            ResetBoss();
            return;
        }
        if (distance <= detectRange && !isUIActivated)
        {
            isUIActivated = true;
            if (hpBarObject != null) hpBarObject.SetActive(true);
            UpdateBossHPUI();
        }
        if (isActionPlaying) return;

        LookAtPlayer();
        DetermineBehavior(distance);
    }

    void UpdateBossHPUI()
    {
        if (hpFill != null)
        {
            hpFill.fillAmount = (float)currentHp / maxHp;
        }
    }
    public void ResetBoss()
    {
        isUIActivated = false;
        currentHp = maxHp;

        if (hpBarObject != null)
        {
            hpBarObject.SetActive(false);
        }
        UpdateBossHPUI();
        transform.position = startPos;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
        ChangeState(State.Idle);
    }

    void DetermineBehavior(float distance)
    {
        if (distance <= attackRange)
        {
            if (currentState != State.ComboAttack) ChangeState(State.ComboAttack);
        }
        else if (distance <= loseSightRange && isUIActivated)
        {
            ChangeState(State.MoveToPlayer);

            stateTimer += Time.deltaTime;
            if (distance >= skillRange && stateTimer > 4f)
            {
                stateTimer = 0f;
                ChangeState(State.SkillAttack);
            }
        }
        else
        {
            if (currentState != State.Idle) ChangeState(State.Idle);
        }
    }

    void ChangeState(State newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case State.Idle:
                PlayAnim(animIdle, true);
                break;

            case State.MoveToPlayer:
                float distance = Vector2.Distance(transform.position, player.position);
                if (distance > skillRange)
                {
                    PlayAnim(animRun, true);
                    Move(runSpeed);
                }
                else
                {
                    PlayAnim(animRun2, true);
                    Move(walkSpeed);
                }
                break;

            case State.ComboAttack:
                isActionPlaying = true;
                string randomAttack = "";
                float currentDelay = 0f;
                float currentDuration = 0f;
                if (Random.Range(0, 2) == 0)
                {
                    randomAttack = animAttack;
                    currentAttackDamage = damageAttack;
                    currentDelay = delayAttack;
                    currentDuration = durationAttack;
                }
                else
                {
                    randomAttack = animAttack2;
                    currentAttackDamage = damageAttack2;
                    currentDelay = delayAttack2;
                    currentDuration = durationAttack2;
                }

                StartCoroutine(EnableAttackColliderRoutine(currentDelay, currentDuration));

                var attackTrack = PlayAnim(randomAttack, false);
                if (attackTrack != null) attackTrack.Complete += OnActionComplete;
                else isActionPlaying = false;
                break;

            case State.SkillAttack:
                isActionPlaying = true;
                string randomSkill = "";
                float currentSkillDelay = 0f;
                float currentSkillDuration = 0f;

                if (Random.Range(0, 2) == 0)
                {
                    randomSkill = animSkill;
                    currentAttackDamage = damageSkill;
                    currentDelay = delaySkill;
                    currentDuration = durationSkill;
                }
                else
                {
                    randomSkill = animGap;
                    currentAttackDamage = damageGap;
                    currentDelay = delayGap;
                    currentDuration = durationGap;
                }

                    StartCoroutine(EnableAttackColliderRoutine(currentSkillDelay, currentSkillDuration));

                var skillTrack = PlayAnim(randomSkill, false);
                if (skillTrack != null) skillTrack.Complete += OnActionComplete;
                else isActionPlaying = false;
                break;
        }
    }

    void Move(float speed)
    {
        Vector3 dir = (player.position - transform.position).normalized;
        dir.y = 0;
        transform.position += dir * speed * Time.deltaTime;
    }

    void LookAtPlayer()
    {
        if (player.position.x < transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);
    }

    void OnActionComplete(Spine.TrackEntry trackEntry)
    {
        trackEntry.Complete -= OnActionComplete;
        isActionPlaying = false;
        ChangeState(State.Idle);
    }

    IEnumerator EnableAttackColliderRoutine(float delayTime, float durationTime)
    {
        yield return new WaitForSeconds(delayTime);
        if (attackTrigger != null) attackTrigger.enabled = true;

        yield return new WaitForSeconds(durationTime);
        if (attackTrigger != null) attackTrigger.enabled = false;
    }

    public void TakeDamage(int damage, Vector2 attackDir)
    {
        if (currentState == State.Dead) return;

        currentHp -= damage;

        UpdateBossHPUI();

        if (hpFill != null) hpFill.fillAmount = (float)currentHp / maxHp;


        if (audioSource != null && hitSfx != null)
        {
            audioSource.PlayOneShot(hitSfx);
        }

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(attackDir * knockbackPower, ForceMode2D.Impulse);
        }

        if (currentHp <= 0)
        {
            Die();
            return;
        }

        isActionPlaying = true;
        currentState = State.Hit;
        var hitTrack = PlayAnim(animDamage, false);
        if (hitTrack != null)
        {
            hitTrack.Complete += OnActionComplete;
        }
        else
        {
            isActionPlaying = false;
        }
    }

    void Die()
    {
        currentState = State.Dead;
        PlayAnim(animDeath, false);

        if (audioSource != null && dieSfx != null)
        {
            audioSource.PlayOneShot(dieSfx);
        }

        if (attackTrigger != null) attackTrigger.enabled = false;
        if (bodyCollider != null) bodyCollider.enabled = false;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        if (hpBarObject != null) hpBarObject.SetActive(false);

        if (GameManager.Instance != null && GameManager.Instance.playerStats != null)
        {
            GameManager.Instance.playerStats.AddExp(expReward);
        }
    }

    Spine.TrackEntry PlayAnim(string name, bool loop)
    {
        if (currentAnim == name) return skeletonAnimation.AnimationState.GetCurrent(0);
        currentAnim = name;
        return skeletonAnimation.AnimationState.SetAnimation(0, name, loop);
    }

    public int GetCurrentDamage()
    {
        return currentAttackDamage;
    }
}
