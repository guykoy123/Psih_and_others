using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun
{

    int rarity_code;
    string rarity;

    Gun_Types Type;

    int fire_rate;
    int damage;
    public float get_fire_rate()
    {
        return this.fire_rate;
    }
    public int get_damage()
    {
        return this.damage;
    }
}
