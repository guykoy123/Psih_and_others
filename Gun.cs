using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun
{
    //rarity codes: 0 - common (default), 1 - uncommon, 2 - rare, 3 - legendary
    int rarity_code;
    string rarity_name;

    Gun_Types Type;

    //firing mode: 0 - semi and auto, 1 - semi
    int firing_mode = 0; 

    float fire_rate;
    float damage;
    //TODO: add accuracy

    public Gun(int type,int rarity = 0, int fire_mode = 0)
    {
        //create Gun_Types object
        Type = new Gun_Types(type);

        //check fire mode legal and save
        Set_Firing_Mode(fire_mode);

        //check rarity and save
        Set_Rarity_Name(rarity);

        //randomize gun stats based on rarity and type
        Randomize_Stats();



    }

    private void Randomize_Stats()
    {
        /*
        each gun type has stat range 
        the stats will be randomized from that range
        the value will be multiplied based on the rarity
        */

        //TODO: add rest of gun types

        float[] Rarity_Multiplier = { 1f, 1.1f, 1.25f, 1.5f };

        int[] Pistol_Fire_Rate_Range = { 150, 200 };
        int[] Pistol_Damage_Range = { 10, 50 };

        System.Random rnd = new System.Random();

        switch (Type.Get_Type_Code())
        {
            case 1:
                fire_rate = rnd.Next(Pistol_Fire_Rate_Range[0], Pistol_Fire_Rate_Range[1]) * Rarity_Multiplier[rarity_code];
                damage= rnd.Next(Pistol_Damage_Range[0], Pistol_Damage_Range[1]) * Rarity_Multiplier[rarity_code];
                break;
        }

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

    public override string ToString()
    {
        string text = "";
        text += "Type: " + Type.Get_Type_Name();
        text += ", Rarity: " + rarity_name;
        text += ", Damage: " + damage.ToString();
        text += ", Fire Rate: " + fire_rate.ToString();
        text += ", Firing mode: " + firing_mode;
        return text;
    }
}
