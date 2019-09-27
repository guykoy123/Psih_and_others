using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour{

    private float BaseHealth;
    private float CurrentHealth;
    private float Defense;
    private float Damage;
    private float Speed;
    private float JumpForce;
    private string EnemyName;

    private bool CanHeal = false; //stores if the enemy can heal
    private float LastHealTime=0f; //strores the last time enemy healed
    private float HealDelay = 3f; //time in seconds between each heal

    private GameObject EnemyUI; //stores the EnemyUI object (has all the UI elemnets as children)
    private GameObject PlayerCamera; //stores the player camera (to update rotation of UI)

    

    private void Start()
    {
        EnemyUI = GameObject.Find("EnemyUI"); //get the EnemyUI object
        PlayerCamera = GameObject.Find("PlayerCamera"); //get the player camera object
    }
    private void Update()
    {
        EnemyUI.transform.LookAt(PlayerCamera.transform); //rotate EnemyUI to face player camera
        EnemyUI.transform.Rotate(new Vector3(0, 180, 0)); //rotate EnemyUI so text will not be backwards

        string text_box = EnemyName + "\n" + "Health:" + (int)CurrentHealth + "/" + (int)BaseHealth; //generate text for the text box (name, health info [as whole numbers])
        EnemyUI.GetComponentInChildren<TextMesh>().text = text_box; //update textbox

        if (Time.time - LastHealTime >= HealDelay) //if enough time has passed since healing
            CanHeal = true; //enemy can heal
    }

    public bool Hit(float Damage) 
    {
        //reduces current enemy health based on Damage and Defense
        //returns true when enemy still health after hit
        //returns false when enemy has died
        float amount = Damage - Defense; //calculate how much health needs to be reduced
        if (amount>0f) //check that amount is positive (Defense does not exceed Damage)
            CurrentHealth -= amount; //reduce amount from current health

        if (CurrentHealth > 0)//check if enemy has health
        {
            Debug.Log(EnemyName + " health: " + CurrentHealth);
            return true; //return true because enemy is alive
        }
            
        else
        {
            Debug.Log(EnemyName + " has died");
            return false; //return false because enemy has died
        }
    } 

    public bool Heal(float amount) 
    {
        //heals enemy
        //does not exceed the base health

        if(CurrentHealth < BaseHealth && CanHeal) //check if current health is lower than base health (maximum health)
        {
            CurrentHealth += amount; //add healing amount to current health
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, BaseHealth); //clamp current health to not exceed base health
            Debug.Log(name + " healed: " + amount);
            CanHeal = false; //because enemy just healed, set CanHeal to false
            LastHealTime = Time.time; //update last healing time
        }
        return CanHeal;
    }

    //get/set function for all variables
    public float GetBaseHealth() { return BaseHealth; } //return base enemy health
    public void SetBaseHealth(float health) { BaseHealth = Mathf.Max(health, 0); }//set base health (minimum value is 0)
    public float GetCurrentHealth() { return CurrentHealth; } //return current enemy health
    public void SetCurrentHealth(float health) { CurrentHealth = Mathf.Clamp(health, 0f, BaseHealth); }//set current health (between 0 and base enemy health)
    public float GetDefense() { return Defense; } //return enemy Defense
    public void SetDefense(float def) { Defense = Mathf.Max(def, 0f); } //set Defense (minimum value is 0)
    public float GetDamage() { return Damage; } //return enemy Damage
    public void SetDamage(float d) { Damage = Mathf.Max(d, 0f); }//set enemy Damage (minimum value is 0)
    public float GetSpeed() { return Speed; } //return enemy Speed
    public void SetSpeed(float s) { Speed = Mathf.Max(0f, s); } //set enemy Speed (minimum value is 0)
    public float GetJumpForce() { return JumpForce; }//return enemy jump force
    public void SetJumpForce(float f) { JumpForce = Mathf.Max(0f, f); } //set jump force (minimum value is 0)
    public string GetName() { return EnemyName; }//return enemy name
    public void SetName(string n) { EnemyName = n; } //set enemy name
    public void SetHealDelay(float delay) { HealDelay = delay; } //set heal delay time
    public float GetHealDelay() { return HealDelay; }//return enemy heal delay time

}
