using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementCharacterController : MonoBehaviour {


    [SerializeField]
    private float rotateSpeed = 6.0f;
    [SerializeField]
    private float speed = 6.0f;
    [SerializeField]
    private float jumpSpeed = 8.0f;
    [SerializeField]
    private float gravity = 20.0f;
    [SerializeField]
    private int maxJumpCap = 1;

    private int currentJumpCount = 0;

    private Vector3 moveDirection = new Vector3(0, 0, 0);
    private CharacterController controller;
    private float mauseInitialPos = 0.0f;
    private float currentRotation = 0.0f;
    private float inputAxisX;
    private float inputAxisY;
    private float inputAxisZ;
    private bool isAtacking = false;
    private Animator playerAnimator;

    public void setIsAtacking(bool atacking) { isAtacking = atacking; }

    void Awake()
    {
        controller = this.gameObject.GetComponent<CharacterController>();
        playerAnimator = GetComponentInChildren<Animator>();
        if (!controller)
        {
            Debug.LogError("Failed on get the Player controller");
        }


    }


    void Update()
    {
        /*if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle1"))
            isAtacking = false;*/

        GetImput(ref inputAxisX,ref inputAxisY,ref inputAxisZ);
        if (!isAtacking)
        {
            Move();
            Rotate();
        }            
        MovementAnimation();
        ResetJumpCount();
        


    }

    private void MovementAnimation()
    {
        if(inputAxisZ == 0 && inputAxisX == 0)
        {
            playerAnimator.SetBool("IsMoving", false);
            playerAnimator.SetFloat("HorizontalMovement", 0);
            playerAnimator.SetFloat("VerticalMovement", 0);
        }
        else
        {
            playerAnimator.SetBool("IsMoving", true);        
            playerAnimator.SetFloat("HorizontalMovement", inputAxisX);
            playerAnimator.SetFloat("VerticalMovement", inputAxisZ);
        }

    }

    private void Rotate()
    {
        if (Input.GetMouseButtonDown(1))
        {
            mauseInitialPos = Input.mousePosition.x;
        }
        if (Input.GetMouseButton(1))
        {
            currentRotation = ((Input.mousePosition.x - mauseInitialPos)/(Screen.width/2));
        }
        

        if (Input.GetAxis("RigthtStickX") != 0)
        {           
            currentRotation = Input.GetAxis("RigthtStickX") ;
        }

        transform.Rotate(0, rotateSpeed * currentRotation * Time.deltaTime, 0);
        currentRotation = 0;
    }

    private void ResetJumpCount()
    {
        if (controller.isGrounded)
        {
            currentJumpCount = 0;            
        }
    }

    private void Move()
    {
        moveDirection = new Vector3(inputAxisX, 0, inputAxisZ);
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed;

        if (!controller.isGrounded)
        {
            inputAxisY -= gravity * Time.deltaTime;            
        }

        moveDirection.y = inputAxisY;

        controller.Move(moveDirection*Time.deltaTime);             

    }

    private void GetImput(ref float inputX,ref float inputY,ref float inputZ)
    {

        inputX = Input.GetAxis("Horizontal");
        

        inputZ = Input.GetAxis("Vertical");
        

        if (Input.GetButtonDown("Jump"))
        {            
            Jump(ref inputY);
        }

        if (Input.GetButtonUp("Fire1") && controller.isGrounded)
        {
            Atack();
        }

    }

    private void Atack()
    {
        playerAnimator.SetTrigger("Atack");
        isAtacking = true;
    }

    private void Jump(ref float inputY)
    {
        if (currentJumpCount < maxJumpCap)
        {
            inputY = jumpSpeed;
            playerAnimator.SetTrigger("Jump");
            currentJumpCount++;
        }
    }
}