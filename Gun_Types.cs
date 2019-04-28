using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Types
{
    string type_name = ""; //can be: pistol - 1, shotgun - 2, smg - 3, assault rifle - 4, lmg - 5, sniper - 6, rpg - 7
    int type_code = 0;
    int firing_mode = 0; // 0 - semi and auto, 1 - semi

    public Gun_Types(int type, int fire_mode=0)
    {
        /* 
        in:  gun type (by number) and firing mode (defaul is auto and semi)
        gun types: pistol - 1, shotgun - 2, smg - 3, assault rifle - 4, lmg - 5, sniper - 6, rpg - 7
        firing modes: 0 - semi and auto, 1 - semi 
        */

        Set_gun_Type(type); //save gun type if lega;

        if (fire_mode > 1 || fire_mode < 0) //check if fire mode out of range
        {
            throw new System.ArgumentException("Firing mode must be 0 or 1", "fire_mode"); // throw exception
        }
        else
            this.firing_mode = fire_mode; //save fire mode

    }

    private void Set_gun_Type(int type)
    {
        //checks that given type is legal and saves it
        //if type is not legal throws an exception
        switch (type)
        {
            case 1:
                this.type_name = "Pistol";
                this.type_code = type;
                break;
            case 2:
                this.type_name = "Shotgun";
                this.type_code = type;
                break;
            case 3:
                this.type_name = "SMG";
                this.type_code = type;
                break;
            case 4:
                this.type_name = "Assault Rifle";
                this.type_code = type;
                break;
            case 5:
                this.type_name = "LMG";
                this.type_code = type;
                break;
            case 6:
                this.type_name = "Sniper";
                this.type_code = type;
                break;
            case 7:
                this.type_name = "RPG";
                this.type_code = type;
                break;
            default:
                throw new System.ArgumentException("Type must be a number between 1 and 7", "type");
        }

   
    }

    public int Get_Type_Code() { return this.type_code; }
    public string Get_Type_Name() { return this.type_name; }
    public int Get_Firing_Mode() { return this.firing_mode; }
}
