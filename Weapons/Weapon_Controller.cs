using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Controller : MonoBehaviour {

    Gun Equipped_Gun;

    public GameObject Fire_Point;

    //TODO: load object from source not manualy
    public GameObject Muzzle_Flash;
    public GameObject Enviroment_Hit_vfx;
    public GameObject Enemy_Hit_vfx;

    public Animator Gun_Animator;
    public Animator Hit_Marker_Animator;

    private bool Aiming = false; //tracks if player is aiming
    private float Zoom_Rate = 80f; //stores the zoom rate (higher number is faster zoom)
    private float Base_FOV; //stores the base FOV of the player

    List<GameObject> particle_systems = new List<GameObject>();

    private float time_to_fire = 0f;

    public Camera Player_Camera;
    public Transform Player_Camera_Transform;
    
    // Use this for initialization
    void Start () {
        //Player_Camera = GetComponentInParent<Transform>();
        Gun_Animator = GetComponent<Animator>();
        Base_FOV = Player_Camera.fieldOfView; //get player FOV
	}
	
	// Update is called once per frame
	void Update ()
    {
        //check if gun equipped
        if (this.Equipped_Gun != null)
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
        }

        //animate zoom in/out of aiming
        if (Aiming) //check that player is aiming
            Player_Camera.fieldOfView = Mathf.MoveTowards(Player_Camera.fieldOfView, Base_FOV - Equipped_Gun.Get_Zoom_Value(), Zoom_Rate * Time.deltaTime); //zoom in to the base player FOV - the gun zoom, at spesific zoom rate
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
        RaycastHit hit;
        if (Physics.Raycast(Player_Camera_Transform.position, Player_Camera_Transform.forward, out hit))
        {
            


            if (hit.transform.tag == "Enemy") //check if enemy is hit
            {
                Enemy enemy = hit.transform.GetComponent<Enemy>(); //get enemy controller component
                Debug.Log("Hit enemy: " + enemy.Get_Name());
                enemy.Hit(Equipped_Gun.Get_Damage()); //call hit on enemy
                Hit_Marker_Animator.SetTrigger("Hit"); //trigger hit marker animation (placeholder animation)
                particle_systems.Add((GameObject)Instantiate(Enemy_Hit_vfx, hit.point, Quaternion.LookRotation(hit.normal))); //play hit effect for enemy
            }
            else //if missed enemy
            {
                Debug.Log("hit:" + hit.transform.name); //print object hit
                particle_systems.Add((GameObject)Instantiate(Enviroment_Hit_vfx, hit.point, Quaternion.LookRotation(hit.normal))); //play hit effect for enviroment
            }
               
        }
        GameObject muzzle_flash = (GameObject)Instantiate(Muzzle_Flash, Fire_Point.transform.position, Quaternion.identity); //instatiate muzzle flash
        muzzle_flash.transform.parent = Fire_Point.transform; //set as child of Fire_Point (will follow its position)
        particle_systems.Add(muzzle_flash); //add to particle system list

    }

    public void Set_Gun(Gun new_gun) { this.Equipped_Gun = new_gun; }
}
