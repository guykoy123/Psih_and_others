using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Controller : MonoBehaviour {

    bool automatic = true;
    float damage = 10f;
    int firing_rate =600;

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
        //primary fire
        if (Input.GetButton("Fire1") && Time.time >= time_to_fire)
        {
            Fire();
            time_to_fire = Time.time +( 1 / (firing_rate * Time.deltaTime));
        }

        //TODO: add aiming

        if (particle_systems.Count > 0)
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
            Debug.Log(hit.transform.name);
        }

        particle_systems.Add((GameObject)Instantiate(Muzzle_Flash, Fire_Point.transform.position, Quaternion.identity));

    }
}
