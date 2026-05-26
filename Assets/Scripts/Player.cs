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
    [SerializeField] private float dashPower = 10f;
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private Transform dustpoint;
    [SerializeField] private GameObject dashdustPrefab;
    [SerializeField] private float dustOffsetX = 0.5f;
    private bool _isDashing;
    private bool canDash = true;
    public bool _isFacingRight = true;

    private Animator _animator;
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _renderer;
    private PlayerHealth playerHealth;

    private bool _isGrounded = true;

    private float h;
    private float moveInput;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();
        playerHealth = GetComponent<PlayerHealth>();
    }
    void Update()
    {
        CheckGround();
        if (playerHealth.IsHit())
            return;

        h = Input.GetAxisRaw("Horizontal");
        moveInput = Input.GetAxisRaw("Horizontal");
        Jump();
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            _animator.SetTrigger("isAttack");
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _animator.SetTrigger("isAttack2");
        }
        if (moveInput > 0)
        {
            _isFacingRight = true;
        }
        else if (moveInput < 0)
        {
            _isFacingRight = false;
        }
        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            StartCoroutine(Dash());
        }
        Vector3 pos = attackPoint.localPosition;

        if (_renderer.flipX)
        {
            pos.x = -Mathf.Abs(pos.x);
        }
        else
        {
            pos.x = Mathf.Abs(pos.x);
        }

        attackPoint.localPosition = pos;

        Animation();

    }
    void FixedUpdate()
    {
        if (_isDashing || playerHealth.IsHit())
            return;
        Move();
    }
    void Move()
    {
        if (_isDashing)
            return;

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
        bool isMoving = Mathf.Abs(h) > 0.1f;

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
        }
    }
    void CheckGround()
    {
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        _animator.SetBool("isGrounded", _isGrounded);
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
    IEnumerator Dash()
    {
        canDash = false;
        _isDashing = true;
        
        Vector3 dustPos = dustpoint.position;
        if (_renderer.flipX)
        {
            dustPos.x += dustOffsetX;
        }
        else
        {
            dustPos.x -= dustOffsetX;
        }
        GameObject dust = Instantiate(dashdustPrefab,dustPos,Quaternion.identity);
        Vector3 scale = dust.transform.localScale;

        if (_renderer.flipX)
        {
            scale.x = -Mathf.Abs(scale.x);
        }
        else
        {
            scale.x = Mathf.Abs(scale.x);
        }

        dust.transform.localScale = scale;
        _animator.SetTrigger("isDash");
        float dir;
        if (_renderer.flipX)
        {
            dir = -1f;
        }
        else
        {
            dir = 1f;
        }
        _rigidbody.linearVelocity = new Vector2(dir * dashPower, _rigidbody.linearVelocity.y);
        
        yield return new WaitForSeconds(dashTime);
        _isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    
    public bool Get_isFacingRight()
    {
        return _isFacingRight;
    }
}
