using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour{

    private float base_health;
    private float current_health;
    private float defense;
    private float damage;
    private float speed;
    private float jump_force;
    private string enemy_name;

    private GameObject Enemy_UI; //stores the Enemy_UI object (has all the UI elemnets as children)
    private GameObject Player_Camera; //stores the player camera (to update rotation of UI)

    private void Start()
    {
        Enemy_UI = GameObject.Find("Enemy_UI"); //get the Enemy_UI object
        Player_Camera = GameObject.Find("Player_Camera"); //get the player camera object
    }
    private void Update()
    {
        Enemy_UI.transform.LookAt(Player_Camera.transform); //rotate Enemy_UI to face player camera
        Enemy_UI.transform.Rotate(new Vector3(0, 180, 0)); //rotate Enemy_UI so text will not be backwards

        string text_box = enemy_name + "\n" + "Health:" + current_health + "/" + base_health; //generate text for the text box (name, health info)
        Enemy_UI.GetComponentInChildren<TextMesh>().text = text_box; //update textbox
    }

    public bool Hit(float damage) 
    {
        //reduces current enemy health based on damage and defense
        //returns true when enemy still health after hit
        //returns false when enemy has died
        float amount = damage - defense; //calculate how much health needs to be reduced
        if (amount>0f) //check that amount is positive (defense does not exceed damage)
        {
            current_health -= amount; //reduce amount from current health
        }

        if (current_health > 0)//check if enemy has health
        {
            Debug.Log(enemy_name + " health: " + current_health);
            return true;
        }
            
        else
        {
            Debug.Log(enemy_name + " has died");
            return false; //return false because enemy has died
        }
    } 

    public void Heal(float amount) 
    {
        //heals enemy
        //does not exceed the base health

        if(current_health < base_health) //check if current health is lower than base health (maximum health)
        {
            current_health += amount; //add healing amount to current health
            current_health = Mathf.Clamp(current_health, 0f, base_health); //clamp current health to not exceed base health
            Debug.Log(name + " healed: " + amount);
        }    
    }

    //get/set function for all variables
    public float Get_Base_Health() { return base_health; } //return base enemy health
    public void Set_Base_Health(float health) { base_health = Mathf.Max(health, 0); }//set base health (minimum value is 0)
    public float Get_Current_Health() { return current_health; } //return current enemy health
    public void Set_Current_Health(float health) { current_health = Mathf.Clamp(health, 0f, base_health); }//set current health (between 0 and base enemy health)
    public float Get_Defense() { return defense; } //return enemy defense
    public void Set_Defense(float def) { defense = Mathf.Max(def, 0f); } //set defense (minimum value is 0)
    public float Get_Damage() { return damage; } //return enemy damage
    public void Set_Damage(float d) { damage = Mathf.Max(d, 0f); }//set enemy damage (minimum value is 0)
    public float Get_Speed() { return speed; } //return enemy speed
    public void Set_Speed(float s) { speed = Mathf.Max(0f, s); } //set enemy speed (minimum value is 0)
    public float Get_Jump_Force() { return jump_force; }//return enemy jump force
    public void Set_Jump_Force(float f) { jump_force = Mathf.Max(0f, f); } //set jump force (minimum value is 0)
    public string Get_Name() { return enemy_name; }//return enemy name
    public void Set_Name(string n) { enemy_name = n; } //set enemy name

}
