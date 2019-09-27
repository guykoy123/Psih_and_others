using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDummyController : MonoBehaviour {
    //heals each time it takes Damage

    Enemy Enemy; //stores the enemy component of object

	// Use this for initialization
	void Start ()
    {
        Enemy = GetComponent<Enemy>(); //get the enemy component

        Enemy.SetBaseHealth(1000f);//set base health to 1000
        Enemy.SetCurrentHealth(1000f);//update current health to base health
        Enemy.SetName("Test Dummy");//set enemy name

        Enemy.SetHealDelay(0.8f); //set low heal delay because dummy can not die
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Enemy.GetCurrentHealth() < Enemy.GetBaseHealth()) //check if recieved Damage
            Debug.Log(Enemy.Heal(Enemy.GetBaseHealth() - Enemy.GetCurrentHealth())); //heal to full health
    }
}
