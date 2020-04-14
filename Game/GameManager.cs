using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public PlayerController p_controller;
    public WeaponController PlayerWeapon;
    public InventoryController PlayerInventory;

    bool OpenMenu = false; //true if a menu is opened
	// Use this for initialization
	void Start ()
    {
        //TESTING INVENTORY SYSTEM
        Gun testGun = new Gun(1,1,1); //create test gun
        Gun testGun2 = new Gun(1, 1, 1);
        Gun testGun3 = new Gun(1,1);
        PlayerInventory.SetGunInIndex(testGun, 0);
        PlayerInventory.SetGunInIndex(testGun2, 1);
        PlayerInventory.SetGunInIndex(testGun3, 2);
        PlayerInventory.EquipGunInIndex(0);
        GameObject test_dummy = GameObject.Find("Test Dummy"); //find the test dummy game object

        //make the cursor invisible
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OpenedMenu()
    {
        //shows cursor and freezes the game
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0;
        OpenMenu = true;
    }

    public void ClosedMenu()
    {
        //make cursor invisible and resume game
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        OpenMenu = false;
    }

    public bool IsMenuOpen() { return OpenMenu; }
}
