using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun
{
    //pistol - 1, shotgun - 2, smg - 3, assault rifle - 4, lmg - 5, sniper - 6, rpg - 7

    //rarity codes: 0 - common (default), 1 - uncommon, 2 - rare, 3 - legendary
    int rarity_code;
    string rarity_name;

    Gun_Types Type;

    //firing mode: 0 - auto, 1 - semi
    int firing_mode = 0; 

    float fire_rate;
    float damage;
    int magazine_size;
    float zoom_value;
    //TODO: add accuracy

    int current_ammo;

    //TODO: load object from source not manualy
    public GameObject Muzzle_Flash;
    public GameObject Hit_Effect;

    public Gun(int type,int rarity = 0, int fire_mode = 0)
    {
        //create Gun_Types object
        Type = new Gun_Types(type);

        //check fire mode legal and save
        Set_Firing_Mode(fire_mode);

        //check rarity and save
        Set_Rarity_Name(rarity);

        //randomize gun stats based on rarity and type
        float[] stats = Gun_Configurations.Generate_Stats(Type.Get_Type_Code(),rarity_code);

        //save stats
        fire_rate = stats[0];
        damage = stats[1];
        magazine_size = (int)stats[2];
        zoom_value = stats[3];

        current_ammo = magazine_size; //reset magazine ammo
    }

    private void Set_Firing_Mode(int fire_mode)
    {
        if (fire_mode > 1 || fire_mode < 0) //check if fire mode out of range
        {
            throw new System.ArgumentOutOfRangeException("fire_mode", "Firing mode must be 0 or 1"); // throw exception
        }
        else
            firing_mode = fire_mode; //save fire mode
    }

    private void Set_Rarity_Name(int rarity)
    {
        //checks if rarity code is legal and updates rarity_name
        switch (rarity)
        {
            case 0:
                rarity_name = "Common";
                break;
            case 1:
                rarity_name = "Uncommon";
                break;
            case 2:
                rarity_name = "Rare";
                break;
            case 3:
                rarity_name = "Legendary";
                break;
            default:
                throw new System.ArgumentOutOfRangeException("rarity", "Must be a value between 0 and 3");
        }
        this.rarity_code = rarity;
    }


    public float Get_Fire_Rate() { return fire_rate; }
    public float Get_Damage() { return damage; }
    public string Get_Rarity() { return rarity_name; }
    public int Get_Fire_Mode() { return firing_mode; }
    public float Get_Zoom_Value() { return zoom_value; }
    public GameObject Get_Muzzle_Flash() { return Muzzle_Flash; }
    public GameObject Get_Hit_Effect() { return Hit_Effect; }


    public override string ToString()
    {
        string text = "";
        text += "Type: " + Type.Get_Type_Name();
        text += ", Rarity: " + rarity_name;
        text += ", Damage: " + damage.ToString();
        text += ", Fire Rate: " + fire_rate.ToString();
        text += ", Firing mode: " + firing_mode;
        text += ",Zoom value: " + zoom_value;
        return text;
    }
}
