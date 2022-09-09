using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector2 movementInput { get; private set; }
    public bool jumpInput { get; private set; }




    private void Update()
    {

        //Horizontal a - d (-1) - 0 - 1
        movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        jumpInput = Input.GetButtonDown("Jump") || Input.GetButton("Jump");

    }
}
