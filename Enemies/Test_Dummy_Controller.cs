using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Dummy_Controller : MonoBehaviour {
    //heals each time it takes damage

    Enemy enemy; //stores the enemy component of object

	// Use this for initialization
	void Start ()
    {
        enemy = GetComponent<Enemy>(); //get the enemy component

        enemy.Set_Base_Health(1000f);//set base health to 1000
        enemy.Set_Current_Health(1000f);//update current health to base health
        enemy.Set_Name("Test Dummy");//set enemy name
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (enemy.Get_Current_Health() < enemy.Get_Base_Health()) //check if recieved damage
            enemy.Heal(enemy.Get_Base_Health() - enemy.Get_Current_Health()); //heal to full health
	}
}
