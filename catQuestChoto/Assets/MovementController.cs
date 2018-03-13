using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour {

    CharacterController controller;
    [SerializeField]
    private float rotateSpeed = 6.0f;
    [SerializeField]
    private float speed = 6.0f;
    [SerializeField]
    private float jumpSpeed = 8.0f;
    [SerializeField]
    private float gravity = 20.0f;

    private Vector3 moveDirection = new Vector3(0,0,0);

    private float mauseInitialPos = 0.0f;
    private float currentRotation = 0.0f;   

    // Use this for initialization
    void Start () {
        controller = this.gameObject.GetComponent<CharacterController>();
    }
	
	// Update is called once per frame
	void FixedUpdate() {
        
        
        
        if (controller.isGrounded)
        {
            moveDirection = new Vector3(0, 0, 0);
            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed ;

        }
        moveDirection.x = Input.GetAxis("Horizontal")* speed;
        moveDirection.z = Input.GetAxis("Vertical")* speed;
        transform.Rotate(0, rotateSpeed * currentRotation, 0);

        moveDirection = transform.TransformDirection(moveDirection);      
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);


    }

    private void OnMouseDown()
    {
        mauseInitialPos = Input.mousePosition.x;
    }

    private void OnMouseDrag()
    {
        currentRotation = ((Input.mousePosition.x - mauseInitialPos)/1000);
    }

    private void OnMouseUp()
    {
        currentRotation = 0;
    }
}
