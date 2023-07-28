using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Player movement parameters
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private LayerMask groundLayer;

    // Components
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private Animator animator;

    private int _idleAnimatorParameter = Animator.StringToHash("Idle");
    private int _runAnimatorParameter = Animator.StringToHash("Run");
    private int _jumpAnimatorParameter = Animator.StringToHash("Jump");
    private int _fallAnimatorParameter = Animator.StringToHash("Fall");

    // Grounded check
    private bool isGrounded = false;
    private float groundedRaycastDistance = 0.02f;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Check for grounded status
        isGrounded = IsGrounded();

        // Horizontal movement
        float horizontalInput = Input.GetAxis("Horizontal");
        if(horizontalInput > 0 )
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (horizontalInput < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        Debug.DrawRay(boxCollider.bounds.center, -transform.up * (groundedRaycastDistance + boxCollider.size.y / 2), Color.red);
        // Jumping
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        AnimationCheck();
        Debug.Log(horizontalInput);
    }

    private bool IsGrounded()
    {
        // Cast a ray just below the player to check for ground
        RaycastHit2D hit = Physics2D.Raycast(boxCollider.bounds.center, Vector2.down, groundedRaycastDistance + boxCollider.size.y / 2, groundLayer);
        return hit.collider != null;
    }

    private void AnimationCheck()
    {
        animator.SetBool(_idleAnimatorParameter, rb.velocity.x == 0 && isGrounded);
        animator.SetBool(_runAnimatorParameter, Mathf.Abs(rb.velocity.x) > 0 && isGrounded);
        animator.SetBool(_jumpAnimatorParameter, rb.velocity.y > 0 && !isGrounded);
        animator.SetBool(_fallAnimatorParameter, rb.velocity.y < 0 && !isGrounded);

    }

}
