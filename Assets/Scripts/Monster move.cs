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

    private Rigidbody2D rb;
    private Animator anim;

    private bool movingRight = true;
    private bool isHit = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        anim.SetBool("Move", true);
    }

    void FixedUpdate()
    {
        if (isHit)
            return;
        CheckGroundAndWall();
        Move();
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

        if (!isGrounded || isWall)
        {
            Flip();
            return;
        }
    }

    void Flip()
    {
        movingRight = !movingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
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
    }
}