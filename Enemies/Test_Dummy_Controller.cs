using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Dummy_Controller : MonoBehaviour {
    //heals each time it takes damage

    Enemy Enemy; //stores the enemy component of object

	// Use this for initialization
	void Start ()
    {
        Enemy = GetComponent<Enemy>(); //get the enemy component

        Enemy.Set_Base_Health(1000f);//set base health to 1000
        Enemy.Set_Current_Health(1000f);//update current health to base health
        Enemy.Set_Name("Test Dummy");//set enemy name
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Enemy.Get_Current_Health() < Enemy.Get_Base_Health()) //check if recieved damage
            Enemy.Heal(Enemy.Get_Base_Health() - Enemy.Get_Current_Health()); //heal to full health
    }
}
