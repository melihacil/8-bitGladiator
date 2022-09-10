using UnityEngine;


[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    
    [SerializeField] private float moveSpeedMax;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;
    [SerializeField] private float velPower;
    private float speed;
    private float targetSpeed;
    private float accelerationRate;
    private float speedDifference;
    private bool playerForward = true;



    [Header("Jump Forces")]

    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCoyoteTime;
    [SerializeField] private float jumpCutMultiplier;
    [SerializeField] private float fallGravityMultiplier;
    private float gravityScale;
    private bool readyDoubleJump = false;
    private bool isGrounded = false;
    private bool jumpButtonReleased = false;
    private bool isJumped = false;


    [Header("Checking Measures")]

    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.5f,0.5f);
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheckPos;

    //Time
    [SerializeField] private float lastGroundedTime;
    //private float lastJumpTime;
    private PlayerInput playerInput;
    private Rigidbody2D rb;

    [Header("Attack Values")]
    [SerializeField] private bool readyToAttack = true;
    [SerializeField] private bool attackButtonReleased = true;
    


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>(); 
        rb = GetComponent<Rigidbody2D>();
        gravityScale = rb.gravityScale;
    }

    private void Start()
    {
        playerForward = true;
    }

    private void Update()
    {
        if (playerInput.movementInput.x != 0)
            CheckDirection(playerInput.movementInput.x > 0);
        lastGroundedTime -= Time.deltaTime;
        isGrounded = Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer);

        if (isGrounded)
        {
            lastGroundedTime = jumpCoyoteTime;
            isJumped = false;
            jumpButtonReleased = false;
            readyDoubleJump = false;
        }
        if (!playerInput.attackInput)
            attackButtonReleased = true;
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
            //Debug.Log("Jumping");
            Jump(1);
            readyDoubleJump = true;
        }
        
        if (playerInput.jumpInput && readyDoubleJump && !isGrounded && jumpButtonReleased)
        {
                
            Debug.Log("Double Jumping");
            Jump(2);
            readyDoubleJump = false;
            jumpButtonReleased = false;
        }
        
        if (!playerInput.jumpInput && isJumped)
        {
            if (readyDoubleJump)
                jumpButtonReleased = true;
            OnJumpUp();
        }
        Move();
        Attack();


    }

    private void Jump(float multiplier)
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        isGrounded=false;
        isJumped = true;
    }

    public void OnJumpUp()
    {
        if (rb.velocity.y > 0)
        {
            rb.AddForce(Vector2.down * rb.velocity.y * (1 - jumpCutMultiplier), ForceMode2D.Impulse);
        }
    }

    private void Attack()
    {
        if (playerInput.attackInput && readyToAttack && attackButtonReleased)
        {
            attackButtonReleased = false;
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector2.right) * 4f, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.right), 4f);
            if (hit)
            {
                //Debug.Log(hit.collider.gameObject.name);
                if (hit.collider.gameObject.layer == 7 )
                {
                    Debug.Log("Attacking = " + hit.collider.gameObject.name);
                    hit.collider.gameObject.GetComponent<BasicEnemyScript>().DamageBackwards();
                }
            }


            readyToAttack = false;
            Invoke(nameof(ResetAttack), 0.5f);
        }
    }



    private void ResetAttack()
    {
        readyToAttack = true;
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

        //Adding the force in the x axis (speed is neg if player moving towards left
        rb.AddForce(Vector2.right * speed);

       //transform.localScale

    }


    private void CheckDirection(bool playerDirection)
    {
        if (playerDirection != playerForward)
        {
            TurnFunction();
        }
    }


    private void TurnFunction()
    {
        Debug.Log("Turning");

        
        /*
         * 
         * If there is any problem with transform.forward then use this code bloc
         * transform.localScale.x can be used to rotate attack direction
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        */
        transform.forward *= -1;
        playerForward = !playerForward;
    }


}
