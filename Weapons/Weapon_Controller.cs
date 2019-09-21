using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon_Controller : MonoBehaviour {

    Gun Equipped_Gun;

    public GameObject Fire_Point;

    //tracks the states of the gun
    private bool Aiming = false; //tracks if player is aiming
    private bool Reloading = false; //tracks if player is reloading

    private float Zoom_Rate = 100f; //stores the zoom rate (higher number is faster zoom)
    private float Base_FOV; //stores the base FOV of the player

    private float time_to_fire = 0f; //stores the time since last shot (used for controlling fire rate)

    //TODO: get from equipped gun (different for every weapon)
    public GameObject Muzzle_Flash;

    public GameObject Enviroment_Hit_vfx; //stores the effect of hitting enviroment (placeholder) need to add different types based on what was hit
    public GameObject Enemy_Hit_vfx; //stores the effect of hitting enemy (placeholder) need to add different types based on what was hit

    public Animator Gun_Animator; //stores the gun animator (used to control gun animations)
    public Animator Hit_Marker_Animator; //stores the hit marker animator (used tp control hit marker animations)

    List<GameObject> particle_systems = new List<GameObject>();

    public Camera Player_Camera;
    public Transform Player_Camera_Transform;

    public Text Ammo_Textbox;

    // Use this for initialization
    void Start () {
        //Player_Camera = GetComponentInParent<Transform>();
        Gun_Animator = GetComponent<Animator>();
        Base_FOV = Player_Camera.fieldOfView; //get player FOV

        //display ammo if gun is equipped
        if(Equipped_Gun != null)
            Update_Ammo();
    }

    // Update is called once per frame
    void Update ()
    {


        //check if gun equipped
        if (Equipped_Gun != null)
        {
            //primary fire
            if (Equipped_Gun.Get_Fire_Mode() == 1 && Input.GetButtonDown("Fire1") && Time.time >= time_to_fire) //check if firing mode is semi and the player pressed fire and it is time to fire
            { 
                Fire();
                time_to_fire = Time.time + (1 / (Equipped_Gun.Get_Fire_Rate() * Time.deltaTime));
            }
            else if (Input.GetButton("Fire1") && Time.time >= time_to_fire && Equipped_Gun.Get_Fire_Mode()!=1) //check if fire button is pressed and its time to fire and fire mode is not semi
            {
                Fire();
                time_to_fire = Time.time + (1 / (Equipped_Gun.Get_Fire_Rate() * Time.deltaTime));
            }

            //aiming
            if (Input.GetButtonDown("Fire2")) //check if aim button down
            {
                Gun_Animator.SetTrigger("Aim"); //trigger aim animation
                Aiming = true; //set aiming to true             
            }
            else if (Input.GetButtonUp("Fire2")) //check if not pressing aim button
            {
                Gun_Animator.SetTrigger("To_Hip"); //trigger to hip animation
                Aiming = false; //set aiming to false
            }

            //reloading
            if(Input.GetButtonDown("Reload") && !Reloading && !Aiming) //check if reload buttons is pressed while not reloading or aiming
            {
                Gun_Animator.SetTrigger("Reload");//trigger reload animation
                Reloading = true; //set reloading to true
            }
            else if(!Gun_Animator.GetCurrentAnimatorStateInfo(0).IsName("Reload") && Reloading) //check if reload animation is finished
            {
                Reloading = false;
                Equipped_Gun.Reload(); //reload current ammo
                Update_Ammo(); //update ammo counter UI
            }
        }

        //animate zoom in/out of aiming
        if (Aiming) //check that player is aiming
            Player_Camera.fieldOfView = Mathf.MoveTowards(Player_Camera.fieldOfView, Base_FOV - Equipped_Gun.Get_Zoom_Value(), Zoom_Rate * Time.deltaTime); //zoom in to (base player FOV - the gun zoom), at spesific zoom rate
        else
            Player_Camera.fieldOfView = Mathf.MoveTowards(Player_Camera.fieldOfView, Base_FOV, Zoom_Rate * Time.deltaTime); //zoom out to the base player FOV, at spesific zoom rate


        if (particle_systems.Count > 0) //removes inactive particle systems to reduce memory usage
            if (!particle_systems[0].GetComponent<ParticleSystem>().IsAlive())
            {
                Destroy(particle_systems[0]);
                particle_systems.RemoveAt(0);
            }

    }


    void Fire()
    {
        if (Equipped_Gun.Shoot())
        {
            RaycastHit hit;
            if (Physics.Raycast(Player_Camera_Transform.position, Player_Camera_Transform.forward, out hit))
            {
                if (hit.transform.tag == "Enemy") //check if enemy is hit
                {
                    Enemy enemy = hit.transform.GetComponent<Enemy>(); //get enemy controller component
                    Debug.Log("Hit enemy: " + enemy.Get_Name());
                    enemy.Hit(Equipped_Gun.Get_Damage()); //call hit on enemy
                    Hit_Marker_Animator.SetTrigger("Hit"); //trigger hit marker animation (placeholder animation)
                    particle_systems.Add((GameObject)Instantiate(Enemy_Hit_vfx, hit.point, Quaternion.LookRotation(hit.normal))); //instatiate hit effect for enemy
                }
                else //if missed enemy
                {
                    Debug.Log("hit:" + hit.transform.name); //print object hit
                    particle_systems.Add((GameObject)Instantiate(Enviroment_Hit_vfx, hit.point, Quaternion.LookRotation(hit.normal))); //instatiate hit effect for enviroment
                }
            }

            GameObject muzzle_flash = (GameObject)Instantiate(Muzzle_Flash, Fire_Point.transform.position, Quaternion.identity); //instatiate muzzle flash
            muzzle_flash.transform.parent = Fire_Point.transform; //set as child of Fire_Point (will follow its position)
            particle_systems.Add(muzzle_flash); //add to particle system list

        }
        else
            Debug.Log("Please Reload");

        Update_Ammo();

    }

    private void Update_Ammo()
    {
        //update ammo counter UI
        Ammo_Textbox.text = Equipped_Gun.Get_Current_Ammo() + " / " + Equipped_Gun.Get_Magazine_Size();
    }

    public void Set_Gun(Gun new_gun)
    {
        //equips new gun and displays the ammo
        Equipped_Gun = new_gun;
        Update_Ammo();
    }
}
