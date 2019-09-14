using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Controller : MonoBehaviour {

    Gun gun;

    public GameObject Fire_Point;

    //TODO: load object from source not manualy
    public GameObject Muzzle_Flash;
    public GameObject Hit_Effect;

    List<GameObject> particle_systems = new List<GameObject>();

    private float time_to_fire = 0f;

    public Transform Player_Camera;
    
    // Use this for initialization
    void Start () {
        //Player_Camera = GetComponentInParent<Transform>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        //check if gun equipped
        if (this.gun != null)
        {
            //primary fire
            if (gun.Get_Fire_Mode() == 1 && Input.GetButtonDown("Fire1") && Time.time >= time_to_fire) //check if firing mode is semi and the player pressed fire and it is time to fire
            {
                Fire();
                time_to_fire = Time.time + (1 / (gun.Get_Fire_Rate() * Time.deltaTime));
            }
            else if (Input.GetButton("Fire1") && Time.time >= time_to_fire && gun.Get_Fire_Mode()!=1) //check if fire button is pressed and its time to fire and fire mode is not semi
            {
                Fire();
                time_to_fire = Time.time + (1 / (gun.Get_Fire_Rate() * Time.deltaTime));
            }
        }
        

        //TODO: add aiming

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
        if (Physics.Raycast(Player_Camera.position, Player_Camera.forward, out hit))
        {
            particle_systems.Add((GameObject)Instantiate(Hit_Effect, hit.point, Quaternion.LookRotation(hit.normal)));


            if (hit.transform.tag == "Enemy") //check if enemy is hit
            {
                Enemy enemy = hit.transform.GetComponent<Enemy>(); //get enemy controller component
                Debug.Log("Hit enemy: " + enemy.Get_Name());
                enemy.Hit(gun.Get_Damage()); //call hit on enemy
            }
            else
                Debug.Log("hit:"+hit.transform.name);
        }
        GameObject muzzle_flash = (GameObject)Instantiate(Muzzle_Flash, Fire_Point.transform.position, Quaternion.identity); //instatiate muzzle flash
        muzzle_flash.transform.parent = Fire_Point.transform; //set as child of Fire_Point (will follow its position)
        particle_systems.Add(muzzle_flash); //add to particle system list

    }

    public void Set_Gun(Gun new_gun) { this.gun = new_gun; }
}
