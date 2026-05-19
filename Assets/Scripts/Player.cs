using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    [Range(1f, 5f)] private float moveSpeed = 1f;

    [SerializeField] private float jumpForce = 1f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private LayerMask monsterLayer;

    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _renderer;

    private bool _isGrounded = true;

    private float h;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        h = Input.GetAxisRaw("Horizontal");

        Jump();
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            _animator.SetTrigger("isAttack");
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _animator.SetTrigger("isAttack2");
        }
        Animation();
    }
    void FixedUpdate()
    {
        Move();
    }
    void Move()
    {
        Vector2 linearVelocity = _rigidbody.linearVelocity;
        linearVelocity.x = h * moveSpeed;
        _rigidbody.linearVelocity = linearVelocity;

        if (h != 0)
        {
            _renderer.flipX = h < 0;
        }
    }
    void Animation()
    {
        bool isMoving = Mathf.Abs(_rigidbody.linearVelocity.x) > 0.05f;

        _animator.SetBool("isMove", isMoving);
        _animator.SetBool("isGrounded", _isGrounded);
    }
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt) &&  _isGrounded)
        {
            _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, jumpForce);

            _animator.SetTrigger("Jump");
            _isGrounded = false;
            _animator.SetBool("isGrounded", false);
        }
    }
    void CheckGround()
    {
        if (_rigidbody.linearVelocity.y > 0.01f)
        {
            _isGrounded = false;
        }
        else
        {
            _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }
    }
    public void AttackHit()
    {
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(attackPoint.position,attackRange,monsterLayer);

        foreach (Collider2D col in hitObjects)
        {
            MonsterHealth monster = col.GetComponent<MonsterHealth>();

            if (monster != null)
            {
                Vector2 attackDir = (monster.transform.position - transform.position).normalized;
                monster.TakeDamage(2, attackDir);
            }
        }
    }
    public void AttackHit2()
    {
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, monsterLayer);

        foreach (Collider2D col in hitObjects)
        {
            MonsterHealth monster = col.GetComponent<MonsterHealth>();

            if (monster != null)
            {
                Vector2 attackDir = (monster.transform.position - transform.position).normalized;
                monster.TakeDamage(4, attackDir);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = false;
        }
    }
}
