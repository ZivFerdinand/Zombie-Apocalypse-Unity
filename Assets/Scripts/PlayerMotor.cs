using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded; //gravity
    public float speed = 5f;
    public float gravity = -9.8f;
    public float jumpHeight = 1f;
    public bool lerpCrouch;
    public bool crouching;
    public float crouchTimer;
    public bool sprinting;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded=controller.isGrounded;
        if (lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float p = crouchTimer / 1;
            p *= p;
            if(crouching)
                controller.height =Mathf.Lerp(controller.height, 1, p);
            else
                controller.height =Mathf.Lerp(controller.height, 2, p);

            if (p > 1)
            {
                lerpCrouch = false;
                crouchTimer = 0f;
            }
        }
    }
    public void ProcessMove(Vector3 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        controller.Move(transform.TransformDirection(moveDirection)*speed*Time.deltaTime);
        playerVelocity.y += gravity * Time.deltaTime;
        if(isGrounded &&playerVelocity.y<0) {
            playerVelocity.y = -2f;
        }
        controller.Move(playerVelocity * Time.deltaTime);
        Debug.Log(playerVelocity.y);
    }
    public void jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
 
        }
    }
    public void Crouch()
    {
        crouching=!crouching;
        crouchTimer=0f;
        if (crouching)
        {
            speed = 2f;
        }
        else
        {
            speed = 5f;
        }
        lerpCrouch=true;
    }
    public void Sprint()
    {
        sprinting=!sprinting;
        if (sprinting)
        {
            speed = 8f;
        }
        else
            speed = 5f;
    }
}
