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
    private bool isClimbing = false; public bool IsClimbing => isClimbing;
    private float horizontalInput;
    private float verticalInput;
    private BoxCollider2D ladderCollider;
    private float overlapWithLadderBelow = 0f;
    private Rigidbody2D rb;
    private BoxCollider2D bc;
    private PlayerEnergy playerEnergy;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        playerEnergy = GetComponent<PlayerEnergy>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        speed = NormalSpeed;
        jumpForce = NormalJumpForce;

        if (DataManager.waitForSpaceKeyUpAtStart)
        {
            DataManager.waitForSpaceKeyUpAtStart = false;
            waitForSpaceKeyUp = true;
        }
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

        if (ladderCollider != null)
        {
            if (overlapWithLadderBelow > 0f && overlapWithLadderBelow < 0.1f)
            {
                isClimbing = verticalInput < 0f;
            }
            else
            {
                isClimbing = verticalInput != 0f;
            }
        }
        if (!isClimbing && (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && isGrounded && !disableJump && !waitForSpaceKeyUp)
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
            ladderCollider = other.GetComponent<BoxCollider2D>();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            float ladderTopY = ladderCollider.bounds.max.y;
            float playerTopY = transform.position.y + bc.bounds.size.y / 2;
            float playerBottomY = transform.position.y - bc.bounds.size.y / 2;
            if (playerTopY > ladderTopY && ladderTopY > playerBottomY)
            {
                overlapWithLadderBelow = (ladderTopY - playerBottomY) / bc.bounds.size.y;
            }
            else
            {
                overlapWithLadderBelow = 0f;
            }
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
            ladderCollider = null;
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
