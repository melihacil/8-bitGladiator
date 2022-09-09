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


    [Header("Checking Measures")]
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.5f,0.5f);
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheckPos;



    //Time
    private float lastGroundedTime;
    private float lastJumpTime;
    

    private float speed;
    private float targetSpeed;
    private float accelerationRate;
    private float speedDifference;
    private PlayerInput playerInput;
    private Rigidbody2D rb;
    private bool jump = false;
    
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>(); 
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {

        lastGroundedTime -= Time.deltaTime;
        if (playerInput.jumpInput)
            jump = true;
        else
            jump = false;

        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer))
            lastGroundedTime = jumpCoyoteTime;
    }

    void FixedUpdate()
    {
        if (lastGroundedTime > 0 && jump)
        {
            Debug.Log("Jumping");
            Jump();
        }
        Move();

    }

    private void Jump()
    {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
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
