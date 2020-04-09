using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using UnityEngine.UI;



public class PlayerController : MonoBehaviour {
    //player stats
    float BaseHealth = 2000f;
    float CurrentHealth = 2000f;
    float Defense = 0f;

    //camera rotation
    float CameraYRotation = 0f; //current camera rotation

    //camera rotation limit angles
    float RotationLimitUp = 90f; //camera up limit rotation
    float RotationLimitDown = -40f; //camera down limit rotation

    //Speed multipliers
    private float Speed = 4f;// base player Speed when walking
    private float SpeedMultiplier = 1f; //current Speed multiplier 

    //Speed multiplier are added to the overall multiplier (for the ability to stack multipliers)
    private float SprintMultiplier =0.6f; //Sprinting Speed multiplier
    private float CrouchMultiplier = -0.4f; //crouch Speed multiplier

    //actual Speed (just for debugging)
    float ActualSpeed = 0f;

    //movement state
    private bool Moving = false;
    private bool Sprinting = false;
    private bool Crouching = false;
    private bool Jump = false;

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
    public Text HealthText;

    private CharacterController CharacterCtrl;

    //animation
    private Animator GunAnimator;
    private Animator PlayerAnimator;

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
        PlayerAnimator = GameObject.Find("PlayerModel").GetComponent<Animator>();

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
        UpdateAnimations(); //update animation to current state
        ActualSpeed = Speed * SpeedMultiplier; //just for debugging

        UpdateHealthText();

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
        if (Input.GetButton("Sprint") && !Crouching && (Input.GetButton("Horizontal") || Input.GetButton("Vertical")))
        {
            if (!Sprinting)
            {
                SpeedMultiplier = SpeedMultiplier + SprintMultiplier; //update multiplier 
                Sprinting = true;
                GunAnimator.SetBool("Sprinting",true);
                PlayerAnimator.SetBool("run", true);
            }
        }
        else if (Sprinting) //if button not pushed and was Sprinting
        {
            SpeedMultiplier = SpeedMultiplier - SprintMultiplier; //return multiplier back to previouse value
            Sprinting = false;
            GunAnimator.SetBool("Sprinting", false);
            PlayerAnimator.SetBool("run", false);
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
            {
                ySpeed = JumpSpeed;
                Jump = true;
            }
                
            else
            {
                ySpeed = Gravity * Time.deltaTime; //if jump button is not pressed apply Gravity
                Jump = false;
            }
                
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


    void UpdateAnimations()
    {
        //update animations based on player state (idle,walk,sprint)
        if (isMoving() && !Jump)
        {
            if (isSprinting())
                PlayerAnimator.SetBool("run", true);
            else if (isCrouching())
                Debug.Log("player crouch animation");
            else
                PlayerAnimator.SetBool("walk", true);
        }
        else
        {
            PlayerAnimator.SetBool("walk", false);
            PlayerAnimator.SetBool("run",false);
        }    
    }


    public void Hit(float damage)
    {
        CurrentHealth -= (damage - Defense);
    }

    void UpdateHealthText()
    {
        HealthText.text = CurrentHealth.ToString() + "/" + BaseHealth.ToString();
        if (CurrentHealth < 0)
            HealthText.text = "You are dead";
    }

    public bool isCrouching() { return Crouching; } //returns if player is crouching
    public bool isMoving() { return Moving; }//returns if player is moving
    public bool isSprinting() { return Sprinting; }//returns if the player is sprinting

    public void TriggerGunEquipAnim()
    {
        /*
         * trigger gun equiping animation
         * TODO: add different animation based on the gun equiped
         */
        PlayerAnimator.SetTrigger("WeaponEquip");
    }


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
