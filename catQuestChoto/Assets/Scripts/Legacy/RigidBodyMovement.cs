using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyMovement : MonoBehaviour {


    [SerializeField]
    private float rotateSpeed = 6.0f;
    [SerializeField]
    private float speed = 6.0f;
    [SerializeField]
    private float jumpSpeed = 8.0f;  
    [SerializeField]
    private int maxJumpCap = 1;

    private int currentJumpCount = 0;

    private Vector3 moveDirection = new Vector3(0, 0, 0);
    private Rigidbody rb;
    private float mauseInitialPos = 0.0f;
    private float currentRotation = 0.0f;
    private float inputAxisX;
    private float inputAxisY;
    private float inputAxisZ;
    

    private

    void Awake()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
        if (!rb)
        {
            Debug.LogError("Failed on get the Player rigidbody");
        }


    }

    private void Update()
    {
        GetImput(ref inputAxisX, ref inputAxisZ);
        ResetJumpCount();
    }

    void FixedUpdate()
    {         
        Move();        
        Rotate();
    }

    private bool isGrounded()
    {
        bool answer = Physics.Raycast(transform.position, Vector3.down, 1);
        return answer;
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
        if (Input.GetMouseButtonUp(1))
        {
            currentRotation = 0;
        }
        Quaternion deltaRotation = Quaternion.Euler(0, rotateSpeed * currentRotation * Time.fixedDeltaTime, 0);
        rb.MoveRotation(rb.rotation*deltaRotation);

    }

    private void ResetJumpCount()
    {
        if(isGrounded())
        {
            currentJumpCount = 0;
        }
    }

    private void Move()
    {
        moveDirection = new Vector3(inputAxisX, 0, inputAxisZ);
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed;

        rb.MovePosition(this.transform.position + (moveDirection * Time.fixedDeltaTime));

     }

    private void GetImput(ref float inputX,ref float inputZ)
    {

        inputX = Input.GetAxis("Horizontal");

        inputZ = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Jump"))
        {            
                Jump();
        }


    }

    private void Jump()
    {
        if (currentJumpCount < maxJumpCap)
        {
            rb.AddForce(Vector3.up * jumpSpeed,ForceMode.Impulse);
            currentJumpCount++;
        }
    }
}