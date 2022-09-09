using UnityEngine;


[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    
    [SerializeField] private float moveSpeedMax;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;
    [SerializeField] private float velPower;
    [Header("Jump Forces")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCoyoteTime;
    [SerializeField] private float jumpCutMultiplier;
    [SerializeField] private float fallGravityMultiplier;
    private float gravityScale;


    [Header("Checking Measures")]
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.5f,0.5f);
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheckPos;



    //Time
    [SerializeField] private float lastGroundedTime;
    //private float lastJumpTime;
    

    private float speed;
    private float targetSpeed;
    private float accelerationRate;
    private float speedDifference;
    private PlayerInput playerInput;
    private Rigidbody2D rb;
    private bool isJumped = false;
    [SerializeField] private bool readyDoubleJump = false;
    [SerializeField] private bool isGrounded = false;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>(); 
        rb = GetComponent<Rigidbody2D>();
        gravityScale = rb.gravityScale;
    }

    private void Update()
    {

        lastGroundedTime -= Time.deltaTime * 10;


        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer))
        {
            lastGroundedTime = 0.1f;
            isJumped = false;
            readyDoubleJump = true;
            isGrounded = true;
        }
    }

    void FixedUpdate()
    {

        if(rb.velocity.y < 0)
        {
            rb.gravityScale = gravityScale * fallGravityMultiplier;
        }
        else
        {
            rb.gravityScale = gravityScale;
        }

        if (lastGroundedTime > 0 && playerInput.jumpInput && !isJumped)
        {
            Debug.Log("Jumping");
            Jump();
        }
        else if (!playerInput.jumpInput && isJumped)
            OnJumpUp();
        
        
        
        if (playerInput.jumpInput && readyDoubleJump && !isGrounded)
        {
            Jump();
            readyDoubleJump=false;
        }

        Move();

    }

    private void Jump()
    {
        //lastJumpTime = 0;
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        isJumped = true;
    }

    public void OnJumpUp()
    {
        if (rb.velocity.y > 0)
        {
            rb.AddForce(Vector2.down * rb.velocity.y * (1 - jumpCutMultiplier), ForceMode2D.Impulse);
        }
    }


    private void Move()
    {
        //Calculate the direction 
        targetSpeed = playerInput.movementInput.x * moveSpeedMax;
        //Calculate the difference between current velocity and desired velocity
        speedDifference = targetSpeed - rb.velocity.x;
        //Change acceleration rate depending on situation 
        accelerationRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration: deceleration;
        
        speed = Mathf.Pow(Mathf.Abs(speedDifference) * accelerationRate, velPower) * Mathf.Sign(speedDifference);


        rb.AddForce(Vector2.right * speed);

    }
}
