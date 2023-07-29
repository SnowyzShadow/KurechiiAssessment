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

    private int moveDirection = 1;
    private List<FruitInventory> fruitInventoryList = new List<FruitInventory>();
    [System.Serializable]
    public class FruitInventory
    {
        public FruitData fruit;
        public int quantity;
    }

    

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
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveDirection = -1;
            transform.localScale = new Vector3(moveDirection, 1, 1);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            moveDirection = 1;
            transform.localScale = new Vector3(moveDirection, 1, 1);
        }
        else
        {
            moveDirection = 0;
        }
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);
        Debug.DrawRay(boxCollider.bounds.center, -transform.up * (groundedRaycastDistance + boxCollider.size.y / 2), Color.red);
        // Jumping
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        AnimationCheck();
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

    private void CollectFruit(GameObject fruitCollided)
    {
        bool hasFruit = false;
        foreach(FruitInventory inventory in fruitInventoryList)
        {
            if (inventory.fruit == fruitCollided.GetComponent<FruitScript>().fruitType)
            {
                inventory.quantity += 1;
                hasFruit = true;
            }
        }
        if (!hasFruit)
        {
            FruitInventory tempInfo = new FruitInventory();
            tempInfo.fruit = fruitCollided.GetComponent<FruitScript>().fruitType;
            tempInfo.quantity = 1;
            fruitInventoryList.Add(tempInfo);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<FruitScript>() != null)
        {
            CollectFruit(collision.gameObject);
            Destroy(collision.gameObject);
        }
    }

}
