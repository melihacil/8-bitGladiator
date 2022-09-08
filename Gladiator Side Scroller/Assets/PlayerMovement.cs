using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    private PlayerInput playerInput;
    private Rigidbody2D rigidbody;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>(); 
    }


    // Update is called once per frame
    void Update()
    {
        Jump();
    }


    private void Jump()
    {
        if (playerInput.jumpInput)
        {
            rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}
