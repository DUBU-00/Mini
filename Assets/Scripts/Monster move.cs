using UnityEngine;

public class MonsterPatrol : MonoBehaviour
{
    [Header("이동")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float patrolWaitTime = 1.5f;

    [Header("돌발 대기 시스템")]
    [SerializeField] private float minWalkDuration = 2f;
    [SerializeField] private float maxWalkDuration = 6f;
    [Range(0f, 1f)]
    [SerializeField] private float turnAroundChance = 0.5f;

    [Header("체크")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float checkDistance = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("추적")]
    [SerializeField] private Transform player;
    [SerializeField] private float chaseDistance = 1f;
    [SerializeField] private float chaseDuration = 3f;

    private Rigidbody2D rb;
    private Animator anim;

    private bool movingRight = true;
    private bool isHit = false;
    private bool canFlip = true;
    private bool isChasing = false;
    private float chaseTimer;

    private bool isWaiting = false;
    private float patrolWaitTimer;
    private float walkTimer;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        ResetWalkTimer();
        anim.SetBool("Move", rb.linearVelocity.x != 0);
    }
    void Update()
    {
        if (anim != null)
        {
            anim.SetBool("Move", Mathf.Abs(rb.linearVelocity.x) > 0.1f);
        }
    }
    void FixedUpdate()
    {
        if (isHit)
            return;

        if (isChasing)
        {
            ChasePlayer();
            float distance = Vector2.Distance(transform.position, player.position);
            if (distance <= checkDistance)
            {
                chaseTimer = chaseDuration;
            }
            else
            {
                chaseTimer -= Time.fixedDeltaTime;
            }

            if (chaseTimer <= 0)
            {
                isChasing = false;
                ResetWalkTimer();
            }
        }
        else
        {
            if (isWaiting)
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                patrolWaitTimer -= Time.fixedDeltaTime;

                if (patrolWaitTimer <= 0)
                {
                    isWaiting = false;
                    if (Random.value < turnAroundChance || IsWallOrCliffAhead())
                    {
                        ExecuteFlip();
                    }
                    ResetWalkTimer();
                }
            }
            else
            {
                CheckGroundAndWall();
                Move();
                walkTimer -= Time.fixedDeltaTime;
                if (walkTimer <= 0)
                {
                    StartWait();
                }
            }
        }
    }
    public void StartChase()
    {
        isChasing = true;
        isWaiting = false;
        chaseTimer = chaseDuration;
    }
    void ChasePlayer()
    {
        float dir = player.position.x > transform.position.x ? 1f : -1f;

        rb.linearVelocity = new Vector2(dir * moveSpeed, rb.linearVelocity.y);

        if ((dir > 0 && !movingRight) || (dir < 0 && movingRight))
        {
            ExecuteFlip();
        }
    }
    public void SetHit(bool value)
    {
        isHit = value;
    }

    void Move()
    {
        float direction = movingRight ? 1f : -1f;

        rb.linearVelocity = new Vector2(direction * moveSpeed,rb.linearVelocity.y);
    }

    void CheckGroundAndWall()
    {
        if (IsWallOrCliffAhead() && canFlip && !isWaiting)
        {
            StartWait();
        }
    }
    bool IsWallOrCliffAhead()
    {

        bool isGrounded = Physics2D.Raycast(groundCheck.position,Vector2.down,checkDistance,groundLayer);

        Vector2 wallDirection = movingRight ? Vector2.right : Vector2.left;

        bool isWall = Physics2D.Raycast(wallCheck.position,wallDirection,checkDistance,groundLayer);

        return !isGrounded || isWall;
    }

    void StartWait()
    {
        isWaiting = true;
        patrolWaitTimer = patrolWaitTime;
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }
    void ResetWalkTimer()
    {
        walkTimer = Random.Range(minWalkDuration, maxWalkDuration);
    }
    void ExecuteFlip()
    {
        if (!canFlip) return;

        canFlip = false;
        movingRight = !movingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        Invoke(nameof(ResetFlip), 0.2f);
    }
    void ResetFlip()
    {
        canFlip = true;
    }
    public void ResetState()
    {
        isHit = false;
        isChasing = false;
        isWaiting = false;
        movingRight = true;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x);
        transform.localScale = scale;

        rb.linearVelocity = Vector2.zero;
        ResetWalkTimer();
    }
    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(groundCheck.position,groundCheck.position + Vector3.down * checkDistance);
        }

        if (wallCheck != null)
        {
            Gizmos.color = Color.blue;
            Vector3 dir = movingRight ? Vector3.right : Vector3.left;
            Gizmos.DrawLine(wallCheck.position,wallCheck.position + dir * checkDistance);
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }
}