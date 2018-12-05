using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {


    [SerializeField] private float rotateSpeed = 6.0f;
    [SerializeField] private float baseSpeed = 6.0f;
    private float speed;
    [SerializeField] private float gravity = 20.0f;
    [SerializeField] GameObject inventory;
    [SerializeField] GameObject questFrame;
    [SerializeField] GameObject statsFrame;
    [SerializeField] GameObject skillTree;
    [SerializeField] GameObject escMenu;
    private Vector3 moveDirection = new Vector3(0, 0, 0);
    private CharacterController controller;
    private float mauseInitialPos = 0.0f;
    private float currentRotation = 0.0f;
    private float inputAxisX;
    private float inputAxisZ;
    private Animator playerAnimator;
    CharacterStats cStats;
    bool casting = false;
    bool killed = false;
    public bool Casting { set { casting = value; } }

    void Awake()
    {
        controller = this.gameObject.GetComponent<CharacterController>();
        if (!controller)
        {
            Debug.LogError("Failed on get the Player controller");
        }
    }
    private void Start()
    {
        speed = baseSpeed;
        cStats = GetComponent<CharacterStats>();
        LoadSystem.OnEndLoading += EndOfLoad;
    }
    private void EndOfLoad()
    {        
        playerAnimator = GetComponentInChildren<Animator>();        
    }
    private void OnDisable()
    {
        LoadSystem.OnEndLoading -= EndOfLoad;
    }



    void Update()
    {
        
        speed = baseSpeed - (baseSpeed * (cStats.status.getDebuffPotency(DebuffType.slow)));

        
        GetImput(ref inputAxisX, ref inputAxisZ);
        
        Move();

        if (cStats.Alive)
        {
            Rotate();
        }
        else
        {
            if (!killed)
            {
                playerAnimator.SetTrigger("Die");
                killed = true;
            }
        }              

        MovementAnimation();

        if (Input.GetKeyDown(KeyCode.I))
        {
            TogleWindow(inventory);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TogleWindow(questFrame);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            TogleWindow(statsFrame);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            TogleWindow(skillTree);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogleWindow(escMenu);
        }

    }

    private void MovementAnimation()
    {
        if(playerAnimator!= null)
        {
            if (inputAxisZ == 0 && inputAxisX == 0)
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
    }   
    private void TogleWindow(GameObject window)
    {
        window.SetActive(!window.activeInHierarchy);
    }
    private void Rotate()
    {
        if (Input.GetMouseButtonDown(1))
        {
            mauseInitialPos = Input.mousePosition.x;
        }
        if (Input.GetMouseButton(1))
        {
            currentRotation = ((Input.mousePosition.x - mauseInitialPos) / (Screen.width / 2));
        }


        if (Input.GetAxis("RigthtStickX") != 0)
        {
            currentRotation = Input.GetAxis("RigthtStickX");
        }

        transform.Rotate(0, rotateSpeed * currentRotation * Time.deltaTime, 0);
        currentRotation = 0;
    }


    private void Move()
    {
        moveDirection = new Vector3(inputAxisX, 0, inputAxisZ);
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed;

        if (!controller.isGrounded)
        {
            moveDirection.y = -gravity * Time.deltaTime;
        }



        controller.Move(moveDirection * Time.deltaTime);

    }

    private void GetImput(ref float inputX, ref float inputZ)
    {
        if (!casting && cStats.Alive)
        {
            inputX = Input.GetAxis("Horizontal");
            inputZ = Input.GetAxis("Vertical");
        }
        else
        {
            inputX = 0;
            inputZ = 0;
            
        }
    }


    

}
