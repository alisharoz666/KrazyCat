using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpForce = 10f;
    public int maxJumps = 2;

    [Header("Ground Check")]
    public Transform groundCheckPoint;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Attack FX")]
    public GameObject pawEffectPrefab;
    public Transform pawSpawnPoint;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private int jumpCount;
    private bool isGrounded;
    private float moveInput;
    private bool facingRight = true;
    public PolygonCollider2D IdleCollider;
    public PolygonCollider2D WalkCollider;
    public PolygonCollider2D SprintCollider;
    float slipForce = 2;
    bool onSlipperySurface = false;
    [Header("Fall Damage")]
    public float fatalFallThreshold = 35f;
    private float jumpStartY;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        jumpCount = 0;
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        HandleAttack();
        CheckGrounded();
    }

    void HandleMovement()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        bool isMoving = moveInput != 0;

        float speed = isRunning ? runSpeed : moveSpeed;
        //rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        if (onSlipperySurface)
        {
            Debug.Log("Slipping");
            rb.AddForce(new Vector2(slipForce, 0f));
        }
        else
        {
            rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        }

        if (moveInput > 0 && !facingRight)
            Flip(true);
        else if (moveInput < 0 && facingRight)
            Flip(false);

        animator.SetBool("isWalking", isMoving && !isRunning);
        animator.SetBool("isRunning", isMoving && isRunning);

        // ⚡ Now use all 3 colliders
        SetColliderState(isMoving, isRunning);
    }

    void SetColliderState(bool isMoving, bool isRunning)
    {
        if (IdleCollider == null || WalkCollider == null || SprintCollider == null) return;

        IdleCollider.enabled = !isMoving;
        WalkCollider.enabled = isMoving && !isRunning;
        SprintCollider.enabled = isMoving && isRunning;
    }


    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumps)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount++;
        }
    }

    void HandleAttack()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            animator.SetTrigger("attack");

            Invoke(nameof(ShowPawEffect), 0.7f);
        }
    }

    void CheckGrounded()
    {
        Collider2D groundHit = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);
        bool wasGrounded = isGrounded;
        isGrounded = groundHit != null;

        if (isGrounded)
        {
            if (!wasGrounded) // just landed
            {
                float fallDistance = jumpStartY - transform.localPosition.y;

                Debug.Log($"Fall Distance: {fallDistance}");

                if (fallDistance >= fatalFallThreshold)
                {
                    if (TryGetComponent<IHealth>(out var health))
                    {
                        health.TakeDamage(100); // Instant death
                    }
                }
            }

            jumpCount = 0;
        }
        else
        {
            if (wasGrounded)
            {
                // Record the height when leaving ground
                jumpStartY = transform.localPosition.y;
            }
        }

        animator.SetBool("isJumping", !isGrounded);
        animator.SetBool("isGrounded", isGrounded);

        Debug.DrawLine(groundCheckPoint.position, groundCheckPoint.position + Vector3.down * 0.1f, isGrounded ? Color.green : Color.red);
    }

    void ShowPawEffect()
    {
        CancelInvoke(nameof(ShowPawEffect));
        GameObject effect = Instantiate(pawEffectPrefab, pawSpawnPoint);
        Destroy(effect, 0.5f);
    }

    void Flip(bool faceRight)
    {
        facingRight = faceRight;

        // Flip the whole cat including pawSpawnPoint
        Vector3 scale = transform.localScale;
        scale.x = faceRight ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Slippery"))
            onSlipperySurface = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Slippery"))
            onSlipperySurface = false;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log("Colliding with: " + collision.collider.name);
    }

}
