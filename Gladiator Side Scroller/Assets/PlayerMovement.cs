using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    private float targetSpeed;

    [SerializeField] private float moveSpeedMax;
    [SerializeField] private float accelariton;
    [SerializeField] private float deceleration;
    [SerializeField] private float velPower;


    private float accelerationRate;
    private float speedDifference;
    private PlayerInput playerInput;
    private Rigidbody2D rb;

    
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>(); 
        rb = GetComponent<Rigidbody2D>();
    }


    // Update is called once per frame
    void Update()
    {
        
    }



    void FixedUpdate()
    {
        Move();
        Jump();
    }

    private void Jump()
    {
        if (playerInput.jumpInput)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void Move()
    {
        //Calculate the direction 
        targetSpeed = playerInput.movementInput.x * moveSpeedMax;
        //Calculate the difference between current velocity and desired velocity
        speedDifference = targetSpeed - rb.velocity.x;
        //Change acceleration rate depending on situation 
        accelerationRate = (Mathf.Abs(targetSpeed) > 0.01f) ? accelariton: deceleration;


        speed = Mathf.Pow(Mathf.Abs(speedDifference) * accelerationRate, velPower) * Mathf.Sign(speedDifference);


        rb.AddForce(Vector2.right * speed);



    }
}
