using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    Gun temp;
    public PlayerController p_controller;
    public WeaponController PlayerWeapon;

	// Use this for initialization
	void Start ()
    {
        temp = new Gun(1,0,1); //create test gun
        PlayerWeapon.EquipGun(temp); // give player the gun

        GameObject test_dummy = GameObject.Find("Test Dummy"); //find the test dummy game object
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public string Test() { return temp.ToString(); }
}
