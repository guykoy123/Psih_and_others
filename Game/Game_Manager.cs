﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour {
    Gun temp;
    public Player_Controller p_controller;
    public WeaponController p_weapon;

	// Use this for initialization
	void Start ()
    {
        temp = new Gun(3,2,0); //create test gun
        p_weapon.Equip_Gun(temp); // give player the gun

        GameObject test_dummy = GameObject.Find("Test Dummy"); //find the test dummy game object
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public string Test() { return temp.ToString(); }
}
