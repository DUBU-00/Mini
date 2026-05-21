using UnityEngine;

public class MonsterPatrol : MonoBehaviour
{
    [Header("이동")]
    [SerializeField] private float moveSpeed = 2f;

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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        anim.SetBool("Move", rb.linearVelocity.x != 0);
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
            }
        }
        else
        {
            CheckGroundAndWall();
            Move();
        }
    }
    public void StartChase()
    {
        isChasing = true;
        chaseTimer = chaseDuration;
    }
    void ChasePlayer()
    {
        float dir = player.position.x > transform.position.x ? 1f : -1f;

        rb.linearVelocity = new Vector2(dir * moveSpeed, rb.linearVelocity.y);

        if ((dir > 0 && !movingRight) || (dir < 0 && movingRight))
        {
            Flip();
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
        bool isGrounded = Physics2D.Raycast(
            groundCheck.position,
            Vector2.down,
            checkDistance,
            groundLayer);

        Vector2 wallDirection = movingRight ? Vector2.right : Vector2.left;

        bool isWall = Physics2D.Raycast(
            wallCheck.position,
            wallDirection,
            checkDistance,
            groundLayer);

        if ((!isGrounded || isWall) && canFlip)
        {
            Flip();
        }
    }

    void Flip()
    {
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
        movingRight = true;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x);
        transform.localScale = scale;

        rb.linearVelocity = Vector2.zero;
    }
    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(
                groundCheck.position,
                groundCheck.position + Vector3.down * checkDistance);
        }

        if (wallCheck != null)
        {
            Gizmos.color = Color.blue;

            Vector3 dir = movingRight ? Vector3.right : Vector3.left;

            Gizmos.DrawLine(
                wallCheck.position,
                wallCheck.position + dir * checkDistance);
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }
}