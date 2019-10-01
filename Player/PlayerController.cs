using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;


public class PlayerController : MonoBehaviour {
    
    //camera rotation
    float CameraYRotation = 0f; //current camera rotation

    //camera rotation limit angles
    float RotationLimitUp = 90f; //camera up limit rotation
    float RotationLimitDown = -40f; //camera down limit rotation

    //Speed multipliers
    private float Speed = 6f;// base player Speed when walking
    private float SpeedMultiplier = 1f; //current Speed multiplier 

    //Speed multiplier are added to the overall multiplier (for the ability to stack multipliers)
    private float SprintMultiplier =0.5f; //Sprinting Speed multiplier
    private float CrouchMultiplier = -0.4f; //crouch Speed multiplier

    //actual Speed (just for debugging)
    float ActualSpeed = 0f;

    //movement state
    private bool Moving = false;
    private bool Sprinting = false;
    private bool Crouching = false;

    //ajustable sesetivity
    [Range(0.1f,5f)] //set value range
    public float CameraSensetivity = 1f; //camera sestivity
    
    //Gravity values
    float Gravity = -15f; //Gravity value
    float ySpeed = 0; //vertical Speed
    float JumpSpeed = 5f;
    float MaxFallSpeed = 50f;

    public Transform PlayerCamera;
    public Transform PlayerMesh;

    private CharacterController CharacterCtrl;

    //animation
    private Animator GunAnimator;

    //member input values (declare input variables public to prevent declaring every frame)
    float xInput;
    float zInput;
    float xMouse;
    float yMouse;

    // Use this for initialization
    void Start ()
    {
        CharacterCtrl = GetComponent<CharacterController>();
        GunAnimator = GameObject.Find("Weapon").GetComponent<Animator>();
    }


    void Update()
    {
        GetInput(); //recieve user movement input
        
        //check if player is moving
        if (xInput != 0 || zInput != 0)
            Moving = true;
        else
            Moving = false;
        UpdateMovement(); //move player

        ActualSpeed = Speed * SpeedMultiplier; //just for debugging
    }

    void GetInput()
    {
        //determines movement type
        //recieves moement input

        GetMovementType(); //get movement type (crouch,sprint)

        //get input from user

        //get keyboard input
        xInput = Speed * SpeedMultiplier * Input.GetAxis("Horizontal");
        zInput = Speed * SpeedMultiplier * Input.GetAxis("Vertical");

        //get mouse input
        xMouse = Input.GetAxis("Mouse X") * CameraSensetivity;
        yMouse = Input.GetAxis("Mouse Y") * CameraSensetivity;
    }

    void GetMovementType()
    {
        //returns the movement type (crouce,sprint,prone)

        //check if sprint button is pushed
        if (Input.GetButton("Sprint") && !Crouching)
        {
            Debug.Log("Sprint");
            if (!Sprinting)
            {
                SpeedMultiplier = SpeedMultiplier + SprintMultiplier; //update multiplier 
                Sprinting = true;
                GunAnimator.SetTrigger("Sprinting");
            }
        }
        else if (Sprinting) //if button not pushed and was Sprinting
        {
            Debug.Log("Stop sprint");
            SpeedMultiplier = SpeedMultiplier - SprintMultiplier; //return multiplier back to previouse value
            Sprinting = false;
            GunAnimator.SetTrigger("StopSprinting");
        }


        //check if crouch button is pushed
        if (Input.GetButton("Crouch") && !Sprinting)
        {
            //Debug.Log("Crouch");
            if (!Crouching)
            {
                SpeedMultiplier = SpeedMultiplier + CrouchMultiplier; //update multiplier 
                Crouching = true;
                PlayerCamera.SetPositionAndRotation(new Vector3(PlayerCamera.position.x, PlayerCamera.position.y - 0.5f, PlayerCamera.position.z),PlayerCamera.rotation); //lower camera
                PlayerMesh.SetPositionAndRotation(new Vector3(PlayerMesh.position.x, PlayerMesh.position.y - 0.5f, PlayerMesh.position.z), PlayerMesh.rotation); //lower player mesh
            }
        }
        else if (Crouching) //if button not pushed and was Crouching
        {
            Debug.Log("Stop crouch");
            SpeedMultiplier = SpeedMultiplier - CrouchMultiplier; //return multiplier back to previouse value
            Crouching = false;
            PlayerCamera.SetPositionAndRotation(new Vector3(PlayerCamera.position.x, PlayerCamera.position.y + 0.5f, PlayerCamera.position.z), PlayerCamera.rotation); //raise camera to original position
            PlayerMesh.SetPositionAndRotation(new Vector3(PlayerMesh.position.x, PlayerMesh.position.y + 0.5f, PlayerMesh.position.z), PlayerMesh.rotation); // raise player mesh to original postition
        }
    }


    void UpdateMovement()
    {
        //move player, camera, and jump

        Vector3 movement = new Vector3(xInput, 0, zInput); //create vector with the movement of the player on the surface
        movement = Vector3.ClampMagnitude(movement, Speed * SpeedMultiplier ); //clamp values to maximum Speed of player (prevents going fast diagonaly because of adding x and z values)
        movement = transform.TransformVector(movement); //rotate movement vector relative to the player rotation

        //check if player is on the ground
        if (CharacterCtrl.isGrounded)
        {
            if (Input.GetButtonDown("Jump") && !Crouching) //if jump button is pressed jump and player is not Crouching
                ySpeed = JumpSpeed;
            else
                ySpeed = Gravity * Time.deltaTime; //if jump button is not pressed apply Gravity
        }
        else
            ySpeed += Gravity * Time.deltaTime; //if not grounded apply Gravity

        //rotate player left and right based on mouse rotation (based on mouse position on the x axis)
        transform.Rotate(0f, xMouse, 0f);

        //rotate player camera up and down within angle limit
        CameraYRotation += yMouse; //add rotation amount (based on mouse movement on y axis)
        CameraYRotation = Mathf.Clamp(CameraYRotation, RotationLimitDown, RotationLimitUp); //clamp rotation to angle limit (if yRotation outside limit returns closest rotation within the limit)
        PlayerCamera.transform.eulerAngles = new Vector3(CameraYRotation, PlayerCamera.eulerAngles.y, 0f); //rotate camera

        //clamp y axis Speed to maximum falling Speed
        ySpeed = Mathf.Clamp(ySpeed, -MaxFallSpeed, MaxFallSpeed);

        //move player on the surface
        CharacterCtrl.Move((movement + new Vector3(0, ySpeed, 0)) * Time.deltaTime); //add vertical Speed to ground movement
    }

    public bool isCrouching() { return Crouching; } //returns if player is crouching
    public bool isMoving() { return Moving; }//returns if player is moving
    public bool isSprinting() { return Sprinting; }//returns if the player is sprinting


    public override string ToString()
    {
        string text = "";

        text += "current Speed: " + ActualSpeed + "("+SpeedMultiplier+")\r\n";

        if (Sprinting)
            text += "movement state: Sprinting\r\n";
        else if (Crouching)
            text += "movement state: Crouching\r\n";
        else
            text += "movement state: walking\r\n";

        return text;
    }
 
}
