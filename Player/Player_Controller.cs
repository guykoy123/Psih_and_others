using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;


public class Player_Controller : MonoBehaviour {
    
    //camera rotation
    float camera_yRotation = 0f; //current camera rotation

    //camera rotation limit angles
    float rotation_limit_up = 90f; //camera up limit rotation
    float rotation_limit_down = -40f; //camera down limit rotation

    //speed multipliers
    private float speed = 6f;// base player speed when walking
    private float speed_multiplier = 1f; //current speed multiplier 

    //speed multiplier are added to the overall multiplier (for the ability to stack multipliers)
    private float sprint_multiplier =0.5f; //sprinting speed multiplier
    private float crouch_multiplier = -0.4f; //crouch speed multiplier
    private float prone_multiplier = -0.7f; //prone speed multiplier

    //actual speed (just for debugging)
    float actual_speed = 0f;

    //movement state
    private bool sprinting = false;
    private bool crouching = false;
    private bool prone = false;

    //ajustable sesetivity
    [Range(0.1f,5f)] //set value range
    public float camera_sensetivity = 1f; //camera sestivity
    
    //gravity values
    float gravity = -15f; //gravity value
    float ySpeed = 0; //vertical speed
    float jumpSpeed = 5f;
    float max_fall_speed = 50f;

    public Transform Player_Camera;
    public Transform Player_Mesh;

    CharacterController cc;

    //member input values (declare input variables public to prevent declaring every frame)
    float xInput;
    float zInput;
    float xMouse;
    float yMouse;

    // Use this for initialization
    void Start ()
    {
        cc = GetComponent<CharacterController>();
    }


    void Update()
    {

        GetInput();
        UpdateMovement();

        actual_speed = speed * speed_multiplier; //just for debugging
    }

    void GetInput()
    {
        GetMovementType(); //get movement type (crouch,sprint,prone)

        xInput = speed * speed_multiplier * Input.GetAxis("Horizontal");
        zInput = speed * speed_multiplier * Input.GetAxis("Vertical");

        xMouse = Input.GetAxis("Mouse X") * camera_sensetivity;
        yMouse = Input.GetAxis("Mouse Y") * camera_sensetivity;
    }

    void GetMovementType()
    {
        //checks the movement type (crouce,sprint,prone)

        //check if sprint button is pushed
        if (Input.GetButton("Sprint") && !crouching)
        {
            Debug.Log("Sprint");
            if (!sprinting)
            {
                speed_multiplier = speed_multiplier + sprint_multiplier; //update multiplier 
                sprinting = true;
            }
        }
        else if (sprinting) //if button not pushed and was sprinting
        {
            Debug.Log("Stop sprint");
            speed_multiplier = speed_multiplier - sprint_multiplier; //return multiplier back to previouse value
            sprinting = false;
        }


        //check if crouch button is pushed
        if (Input.GetButton("Crouch") && !sprinting)
        {
            Debug.Log("Crouch");
            if (!crouching)
            {
                speed_multiplier = speed_multiplier + crouch_multiplier; //update multiplier 
                crouching = true;
                Player_Camera.SetPositionAndRotation(new Vector3(Player_Camera.position.x, Player_Camera.position.y - 0.5f, Player_Camera.position.z),Player_Camera.rotation); //lower camera
                Player_Mesh.SetPositionAndRotation(new Vector3(Player_Mesh.position.x, Player_Mesh.position.y - 0.5f, Player_Mesh.position.z), Player_Mesh.rotation); //lower player mesh
            }
        }
        else if (crouching) //if button not pushed and was crouching
        {
            Debug.Log("Stop crouch");
            speed_multiplier = speed_multiplier - crouch_multiplier; //return multiplier back to previouse value
            crouching = false;
            Player_Camera.SetPositionAndRotation(new Vector3(Player_Camera.position.x, Player_Camera.position.y + 0.5f, Player_Camera.position.z), Player_Camera.rotation); //raise camera to original position
            Player_Mesh.SetPositionAndRotation(new Vector3(Player_Mesh.position.x, Player_Mesh.position.y + 0.5f, Player_Mesh.position.z), Player_Mesh.rotation); // raise player mesh to original postition
        }
    }


    void UpdateMovement()
    {
        //move player and camera, and jump

        Vector3 movement = new Vector3(xInput, 0, zInput); //create vector with the movement of the player
        movement = Vector3.ClampMagnitude(movement, speed * speed_multiplier ); //clamp vector values to speed to prevent going fast diagonaly
        movement = transform.TransformVector(movement); //rotate movement vector relative to the player rotation


        //check if grounded
        if (cc.isGrounded)
        {
            if (Input.GetButtonDown("Jump") && !crouching) //if jump button is pressed jump and player is not crouching
            {
                ySpeed = jumpSpeed;
            }
            else
            {
                ySpeed = gravity * Time.deltaTime; //if jump button is not pressed apply gravity
            }
        }
        else
        {
            ySpeed += gravity * Time.deltaTime; //if not grounded apply gravity
        }

        //rotate player left and right
        transform.Rotate(0f, xMouse, 0f);

        //rotate player camera up and down within an angle limit
        camera_yRotation += yMouse; //add rotation amount based on mouse movement
        camera_yRotation = Mathf.Clamp(camera_yRotation, rotation_limit_down, rotation_limit_up); //return rotation value between the limit (if yRotation outside limit returns closest rotation within the limit)
        Player_Camera.eulerAngles = new Vector3(camera_yRotation, Player_Camera.eulerAngles.y, 0f); //rotate camera

        //clamp horizontal speed to prevent falling too fast
        ySpeed = Mathf.Clamp(ySpeed, -max_fall_speed, max_fall_speed);

        //move player
        cc.Move((movement + new Vector3(0, ySpeed, 0)) * Time.deltaTime); //add vertical speed to ground movement
    }



    public override string ToString()
    {
        string text = "";

        text += "current speed: " + actual_speed + "("+speed_multiplier+")\r\n";

        if (sprinting)
            text += "movement state: sprinting\r\n";
        else if (crouching)
            text += "movement state: crouching\r\n";
        else
            text += "movement state: walking\r\n";

        return text;
    }
 
}
