using UnityEngine;
using Proyecto26;

public class FirebaseTest
{
    public string num;
    public int age;
}

public class PlayerControl : MonoBehaviour
{
    public FirebaseTest firebaseTest = new();

    public bool disableAllAction = false;
    public bool disableJump = false;
    public bool waitForSpaceKeyUp = false;
    private readonly float NormalSpeed = 5f;
    private readonly float NormalJumpForce = 10f;
    private readonly Color NormalColor = Color.white;
    private readonly Color DizzyColor = Color.grey;
    private float speed;
    private float jumpForce;
    private bool isTryingToJump = false;
    private bool isGrounded = true;
    private bool isTouchingCoffeeSpill = false;
    private float horizontalInput;
    private Rigidbody2D rb;
    private PlayerClimb playerClimb;
    private PlayerEnergy playerEnergy;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerClimb = GetComponent<PlayerClimb>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        speed = NormalSpeed;
        jumpForce = NormalJumpForce;
        playerEnergy = GetComponent<PlayerEnergy>();

        firebaseTest.num = "John";
        firebaseTest.age = 14;

        RestClient.Post("https://cs526-senior-associates-default-rtdb.firebaseio.com/test.json", firebaseTest);
        Debug.Log("send test data to firebase");



    }

    void Update()
    {
        SetPlayerSlowDown(isTouchingCoffeeSpill);

        horizontalInput = Input.GetAxis("Horizontal");
        if (waitForSpaceKeyUp && Input.GetKeyUp(KeyCode.Space))
        {
            waitForSpaceKeyUp = false;
        }
        if (Input.GetKey(KeyCode.Space) && isGrounded && !playerClimb.IsClimbing && !disableJump && !waitForSpaceKeyUp)
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
            rb.velocity = new(horizontalVelocity, isTryingToJump ? jumpForce : rb.velocity.y);
        }
        isTryingToJump = false;
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
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("CoffeeSpill"))
        {
            isTouchingCoffeeSpill = false;
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
