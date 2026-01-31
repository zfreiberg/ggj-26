using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System;


[System.Serializable]
public struct PlayerState { public bool isMoving; public bool isJumping; }

// Converted to 2D
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControl : MonoBehaviour
{
    public static Vector2 PlayerLastPosition;
    public static PlayerControl Inst { get; private set; }

    InputSystem_Actions inputActions;
    Rigidbody2D rb;

    [Header("Movement")]
    [SerializeField] float moveForce = 5f;
    [SerializeField] float moveForceAir = 1f;
    [SerializeField] float moveSmoothTime = 0.06f; // smaller = snappier
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float jumpForceDouble = 10f;

    [Header("Ground Check")]
    [SerializeField] float groundCheckDistance = 1f;
    [SerializeField] LayerMask groundLayer = ~0; // default: everything

    [Header("Animation")]
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] bool flipSprite = true;

    [SerializeField] float dashDuration = 0.15f;
    bool isDashing;

    float moveX;
    float velXSmooth; // SmoothDamp ref
    bool isGrounded;
    [SerializeField] PlayerState playerState;

    void Awake()
    {
        Inst = this;
        inputActions = new InputSystem_Actions();
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate; // render-time interpolation
        playerState = new PlayerState { isMoving = false, isJumping = false };

        if (!animator) animator = GetComponentInChildren<Animator>(true);
        if (!spriteRenderer) spriteRenderer = GetComponentInChildren<SpriteRenderer>(true);
        PlayerLastPosition = transform.position;
    }

    void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.Jump.performed += OnJump;
    }

    void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Jump.performed -= OnJump;
        inputActions.Player.Disable();
    }

    void Update()
    {
        // 2D ground check via raycast
        Vector2 origin = (Vector2)transform.position + Vector2.up * 0.1f;
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, groundCheckDistance, groundLayer);
        isGrounded = hit.collider != null;

        var velocity = rb.linearVelocity;
        if (isGrounded && playerState.isJumping && velocity.y <= 0f) 
            playerState.isJumping = false;

        if (animator)
        {
            animator.SetBool("Grounded", isGrounded);
            animator.SetFloat("Speed", Mathf.Abs(moveX));
            animator.SetBool("Jumping", playerState.isJumping);
            // animator.SetFloat("YVel", rb.linearVelocity.y);
        }
        if (flipSprite && spriteRenderer && Mathf.Abs(moveX) > 0.01f)
            spriteRenderer.flipX = moveX < 0f;
    }

    void FixedUpdate()
    {
        // if (playerState.isJumping)
        //     return;
        
        var force = isGrounded ? moveForce : moveForceAir;
        rb.AddForce(new Vector2(moveX * force, 0f), ForceMode2D.Force);

        playerState.isMoving = Mathf.Abs(moveX) > 0.01f;

        PlayerLastPosition = transform.position;
    }

    void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 movementInput = ctx.ReadValue<Vector2>();
        moveX = movementInput.x;
        if (ctx.canceled) moveX = 0f;
    }

    void OnJump(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        // Only jump when on ground and not already in a jump
        if (isGrounded && !playerState.isJumping)
        {
            // Zero vertical velocity so the impulse is consistent
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            var jumpForce = MaskControl.hasGreenMask ? this.jumpForceDouble : this.jumpForce;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            playerState.isJumping = true;
        }
    }

    public void OnEnemyTouch()
    {
        // destroy player unit
        Destroy(gameObject);
        GameControl.Inst.OnGameOver();
    }

    void OnDash(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed || isDashing) return;
        StartCoroutine(DashRoutine());
    }

    IEnumerator DashRoutine()
    {
        isDashing = true;
        animator.SetBool("IsDashing", true);

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;
        animator.SetBool("IsDashing", false);
    }
}
