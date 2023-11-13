using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D call;
    private SpriteRenderer sprite;
    private Animator anim;

    [SerializeField] private LayerMask jumpableGround;

    private float dirX = 0f;
    private float dirY = .1f;
    private bool isJumping = false;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;
    

    private enum MovementState{idle, running, jumping, hit}

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        call = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2 (dirX * moveSpeed, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            isJumping = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        if (Input.GetButtonUp("Jump") && isJumping)
        {
            isJumping = false;
            if (rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            }
        }
        UpdateAnimationState();
    }
    private void UpdateAnimationState()
    {
        MovementState mState;
        if (dirX > 0f)
        {
            mState = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            mState = MovementState.running;
            sprite.flipX = true;
        }
        else
        {
            mState = MovementState.idle;
        }
        if (rb.velocity.y > 0.1f || rb.velocity.y < -0.1f)
        {
            mState = MovementState.jumping;
        }
        anim.SetInteger("State", (int)mState);
    }
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(call.bounds.center, call.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
