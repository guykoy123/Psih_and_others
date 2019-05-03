using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Stats{

    private float base_health;
    private float current_health;
    private float defense;
    private float damage;
    private float speed;
    private Vector3 jump_force;
    private string name;

    public Enemy_Stats(float health,float defense,float damage,float speed,Vector3 jump,string name)
    {
        this.base_health = health;
        this.current_health = health;
        this.defense = defense;
        this.damage = damage;
        this.speed = speed;
        this.jump_force = jump;
        this.name = name;
    }

    public float Get_Health() {return this.current_health;} //return current enemie health

    public void Hit(float damage) //reduce health based on damage and defense
    {
        float amount = damage - defense;
        if (amount>0f)
            this.current_health -= amount;
    } 

    public void Heal(float amount) //heal enemie but not higher then orignal health
    {
        if(this.current_health+amount <= this.base_health)
            this.current_health += amount;
    }


}
