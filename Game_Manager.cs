using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour {
    Gun temp;
    public Player_Controller p_controller;
    public Weapon_Controller p_weapon;
	// Use this for initialization
	void Start ()
    {
        temp = new Gun(1,3,1);
        p_weapon.Set_Gun(temp);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public string Test() { return temp.ToString(); }
}
