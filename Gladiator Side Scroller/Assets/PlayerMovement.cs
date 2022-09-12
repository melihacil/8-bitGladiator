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
    [SerializeField] private LayerMask enemyLayerMask;
    [SerializeField] private Transform groundCheckPos;

    //Time
    [SerializeField] private float lastGroundedTime;
    //private float lastJumpTime;
    private PlayerInput playerInput;
    private Rigidbody2D rb;

    [Header("Attack Values")]
    [SerializeField] private Transform attackPos;
    [SerializeField] private bool readyToAttack = true;
    [SerializeField] private bool attackButtonReleased = true;
    
    [Header("Animator")]
    private Animator animator;
    //[SerializeField] private Animator slideAnimator;
    private float delayToIdle = 0.0f;




    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>(); 
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        gravityScale = rb.gravityScale;
    }

    private void Start()
    {
        playerForward = true;
    }

    private void Update()
    {
        //Running
        if (playerInput.movementInput.x != 0)
        {
            CheckDirection(playerInput.movementInput.x > 0);
            animator.SetInteger("AnimState", 1);
        }
        else
        {
            //Preventing flickering
            delayToIdle -= Time.deltaTime;
            if (delayToIdle < 0.0f)
                animator.SetInteger("AnimState", 0);
        }
        lastGroundedTime -= Time.deltaTime;
        isGrounded = Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer);
        animator.SetBool("Grounded", isGrounded);

        if (isGrounded)
        {
            //Add landing effect here
            if (isJumped)
            {

            }

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
        animator.SetFloat("AirSpeedY", rb.velocity.y);
        //Block for checking gravity
        if (rb.velocity.y < 0)
        {
            //After starting to fall it applies multiplier to improve movement
            rb.gravityScale = gravityScale * fallGravityMultiplier;
            
        }
        else
        {
            rb.gravityScale = gravityScale;
        }
       
        //Jumping block 
        if (lastGroundedTime > 0 && playerInput.jumpInput && !isJumped)
        {
            //Jumping effect should be added here 
           
            Jump(1);
            readyDoubleJump = true;
        }
        
        //Double jumping block
        if (playerInput.jumpInput && readyDoubleJump && !isGrounded && jumpButtonReleased)
        {
            //Double jumping effect  will be added here

            rb.velocity = new Vector3(rb.velocity.x, 0, 0);
            animator.SetInteger("AnimState", 4);
            Debug.Log("Double Jumping");
            Jump(3);
            readyDoubleJump = false;
            jumpButtonReleased = false;
        }
        
        //Gets ready for double jump after jumping and releasing jump button
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
        animator.SetTrigger("Jump");
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

    private int attackState = 1;

    private void Attack()
    {
        if (playerInput.attackInput && readyToAttack && attackButtonReleased)
        {
            attackState++;

            if (attackState > 3)
                attackState = 1;

            animator.SetTrigger("Attack" + attackState);
            attackButtonReleased = false;
            /*
             * Old attack system where you can only hit one enemy at a time
            Debug.DrawRay(attackPos.position, transform.TransformDirection(Vector2.right) * 3f, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(attackPos.position, transform.TransformDirection(Vector2.right), 3f);
            if (hit)
            {
                //Debug.Log(hit.collider.gameObject.name);
                if (hit.collider.gameObject.layer == 7 )
                {
                    Debug.Log("Attacking = " + hit.collider.gameObject.name);
                    hit.collider.gameObject.GetComponent<EnemyScript>().DamageBackwards(transform);
                }
            }

            */
            Collider2D[] collisions = Physics2D.OverlapCircleAll(attackPos.position, 0.5f, enemyLayerMask);
            foreach( Collider2D collision in collisions)
            {
                Debug.Log("Attacking = " + collision.gameObject.name);
                collision.gameObject.GetComponent<EnemyScript>().DamageBackwards(transform);
            }


            readyToAttack = false;
            Invoke(nameof(ResetAttack), 0.2f);
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
