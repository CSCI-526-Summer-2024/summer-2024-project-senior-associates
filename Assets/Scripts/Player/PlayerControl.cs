using UnityEngine;
using Proyecto26;
using System;

public class PlayerControl : MonoBehaviour
{
    public bool disableAllAction = false;
    public bool disableJump = false;
    public bool waitForSpaceKeyUp = false;
    private readonly float NormalSpeed = 5f;
    private readonly float NormalJumpForce = 10f;
    private readonly Color NormalColor = Color.white;
    private readonly Color DizzyColor = Color.grey;
    private readonly float NormalGravity = 4f;
    private float speed;
    private float jumpForce;
    private bool isTryingToJump = false;
    private bool isGrounded = true;
    private bool isTouchingCoffeeSpill = false;
    private bool isNearLadder = false;
    private bool isClimbing = false; public bool IsClimbing => isClimbing;
    private float horizontalInput;
    private float verticalInput;
    private Rigidbody2D rb;
    private PlayerEnergy playerEnergy;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        speed = NormalSpeed;
        jumpForce = NormalJumpForce;
        playerEnergy = GetComponent<PlayerEnergy>();
    }

    void Update()
    {
        SetPlayerSlowDown(isTouchingCoffeeSpill);

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        if (waitForSpaceKeyUp && Input.GetKeyUp(KeyCode.Space))
        {
            waitForSpaceKeyUp = false;
        }

        if (isNearLadder && Mathf.Abs(verticalInput) > 0f)
        {
            isClimbing = true;
        }
        else if ((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && isGrounded && !isClimbing && !disableJump && !waitForSpaceKeyUp)
        {
            isTryingToJump = true;
        }
    }

    void FixedUpdate()
    {
        if (disableAllAction)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else
        {
            var horizontalVelocity = horizontalInput * speed;
            if (playerEnergy != null && playerEnergy.IsSleeping)
            {
                horizontalVelocity = 0f;
            }

            var verticalVelocity = rb.velocity.y;
            if (isClimbing)
            {
                verticalVelocity = verticalInput * NormalSpeed;
            }
            else if (isTryingToJump)
            {
                verticalVelocity = jumpForce;
            }

            rb.velocity = new(horizontalVelocity, verticalVelocity);
        }
        isTryingToJump = false;
        rb.gravityScale = isClimbing ? 0f : NormalGravity;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            ContactPoint2D contact = other.GetContact(0);
            Bounds bounds = GetComponent<Collider2D>().bounds;
            bool isCollisionFromBottom = Vector2.Dot(contact.normal, Vector2.up) > 0.5f;
            if (isCollisionFromBottom)
            {
                isGrounded = true;
            }
        }
    }


    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("CoffeeSpill"))
        {
            isTouchingCoffeeSpill = true;
        }
        else if (other.CompareTag("Ladder"))
        {
            isNearLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("CoffeeSpill"))
        {
            isTouchingCoffeeSpill = false;
        }
        else if (other.CompareTag("Ladder"))
        {
            isNearLadder = false;
            isClimbing = false;
        }
    }

    private void SetPlayerSlowDown(bool slow)
    {
        if (slow || playerEnergy != null && playerEnergy.Tired)
        {
            speed = NormalSpeed / 3;
            jumpForce = NormalJumpForce / 3;
            spriteRenderer.color = DizzyColor;
        }
        else
        {
            speed = NormalSpeed;
            jumpForce = NormalJumpForce;
            spriteRenderer.color = NormalColor;
        }
    }
}
