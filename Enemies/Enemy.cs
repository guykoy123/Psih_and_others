using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour{

    private float BaseHealth;
    private float CurrentHealth;
    private float Defense;
    private float Damage;
    private float HealAmount;
    private float Speed;
    private float JumpForce;
    private string EnemyName;
    private bool Dead = false;
    private bool PlayedDeadAnimation = false;

    private TextMesh EnemyUI; //stores the EnemyUI object (has all the UI elemnets as children)
    private GameObject PlayerCamera; //stores the player camera (to update rotation of UI)

    public GameObject HealParticleSystem;
    public GameObject DamageParticleSystem;

    private GarbageCollector Garbage; //collects garbage to be destroyed to prevent resource hogging

    private void Start()
    {
        //EnemyUI = GameObject.Find("EnemyUI"); //get the EnemyUI object
        EnemyUI = GetComponentInChildren<TextMesh>();
        PlayerCamera = GameObject.Find("PlayerCamera"); //get the player camera object
        Garbage = GameObject.Find("GarbageCollector").GetComponent<GarbageCollector>();
    }
    private void Update()
    {
        EnemyUI.transform.LookAt(PlayerCamera.transform); //rotate EnemyUI to face player camera
        EnemyUI.transform.Rotate(new Vector3(0, 180, 0)); //rotate EnemyUI so text will not be backwards

        string text_box = EnemyName + "\n" + "Health:" + (int)CurrentHealth + "/" + (int)BaseHealth; //generate text for the text box (name, health info [as whole numbers])
        EnemyUI.text = text_box; //update textbox
    }

    public void Hit(float Damage) 
    {
        //reduces current enemy health based on Damage and Defense
        //returns true when enemy still health after hit
        //returns false when enemy has died
        float amount = Damage - Defense; //calculate how much health needs to be reduced
        if (amount>0f) //check that amount is positive (Defense does not exceed Damage)
            CurrentHealth -= amount; //reduce amount from current health

        DamageParticleSystem.GetComponentInChildren<Text>().text = ((int)amount).ToString();//set damage amount for damage indicator (convert to int to remove decimal places)
        Garbage.AddParticleSystem(Instantiate(DamageParticleSystem,transform)); //create damage particle system

        if (CurrentHealth <= 0)//check if enemy is dead
        {
            Debug.Log(EnemyName + " has died");
            Dead = true;
        }
            
            
    } 

    public void Heal() 
    {
        //heals enemy
        //does not exceed the base health

        if(CurrentHealth < BaseHealth) //check if current health is lower than base health (maximum health)
        {
            CurrentHealth += HealAmount; //add healing amount to current health
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, BaseHealth); //clamp current health to not exceed base health
            Debug.Log(name + " healed: " + HealAmount);
            
            HealParticleSystem.GetComponentInChildren<Text>().text=((int)HealAmount).ToString();//set heal amount for heal indicator (convert to int to remove decimal places)
            Garbage.AddParticleSystem(Instantiate(HealParticleSystem,transform)); //create heal particle system
        }
    }

    public bool CanDespwan()
    {
        if (PlayedDeadAnimation)
            return true;
        return false;
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
    public bool IsDead() { return Dead;}//returns if enemy has dies
    public void PlayedDeadAnim() { PlayedDeadAnimation = true; }//called after death animation has been played
    public bool IsPLayedDeadAnim() { return PlayedDeadAnimation; } //returns if death animation has been played
    public void SetHealAmount(float amount) { HealAmount = amount; }
    public float GetHealAmount() { return HealAmount; }


}
