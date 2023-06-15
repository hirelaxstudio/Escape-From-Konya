using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private BoxCollider2D bc;

    private float dirx;
    private float raycastDistance;
    private Vector2 raycastOrigin;
    private float raycastExtentY;

    [SerializeField] private float playerSpeed = 7f;
    [SerializeField] private float playerJump = 14f;
    [SerializeField] private LayerMask groundLayer;

    private enum MovementState { idle, running, jumping, falling }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();

        raycastDistance = 0.1f;
        raycastExtentY = bc.bounds.extents.y;
    }

    private void Update()
    {
        UpdateRaycastOrigin();
        bool isGrounded = IsGroundedCheck();

        // jump
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, playerJump);
        }

        UpdateAnimationState();
    }

    private void FixedUpdate()
    {
        dirx = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirx * playerSpeed, rb.velocity.y);
    }

    private void UpdateRaycastOrigin()
    {
        raycastOrigin = bc.bounds.center;
    }

    private void UpdateAnimationState()
    {
        MovementState state = MovementState.idle;

        if (rb.velocity.y > .1f) // Zýplama animasyonu
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f) // Düþme animasyonu
        {
            state = MovementState.falling;
        }
        else if (Mathf.Abs(dirx) > 0f) // Koþma animasyonu ve Yüzünü çevirme
        {
            state = MovementState.running;
            sr.flipX = (dirx < 0f);
        }

        // Karakter havadayken hareket yönüne göre yüzünü çevirme
        if (Mathf.Abs(rb.velocity.y) > .1f && Mathf.Abs(dirx) > 0f)
        {
            sr.flipX = (dirx < 0f);
        }

        anim.SetInteger("state", (int)state);
    }

    private bool IsGroundedCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.down, raycastExtentY + raycastDistance, groundLayer);

        Debug.DrawRay(raycastOrigin, Vector2.down * (raycastExtentY + raycastDistance), hit.collider != null ? Color.green : Color.red);

        return hit.collider != null;
    }
}
