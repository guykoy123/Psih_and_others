﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Types
{
    string type_name = ""; //can be: pistol - 1, shotgun - 2, smg - 3, assault rifle - 4, lmg - 5, sniper - 6, rpg - 7
    int type_code = 0;

    public Gun_Types(int type)
    {
        /* 
        in:  gun type (by number) and firing mode (defaul is auto and semi)
        gun types: pistol - 1, shotgun - 2, smg - 3, assault rifle - 4, lmg - 5, sniper - 6, rpg - 7
        */

        Set_gun_Type(type); //save gun type if lega;

    }

    private void Set_gun_Type(int type)
    {
        //checks that given type is legal and saves it
        //if type is not legal throws an exception
        switch (type)
        {
            case 1:
                this.type_name = "Pistol";
                break;
            case 2:
                this.type_name = "Shotgun";
                break;
            case 3:
                this.type_name = "SMG";
                break;
            case 4:
                this.type_name = "Assault Rifle";
                break;
            case 5:
                this.type_name = "LMG";
                break;
            case 6:
                this.type_name = "Sniper";
                break;
            case 7:
                this.type_name = "RPG";
                break;
            default:
                throw new System.ArgumentOutOfRangeException("type","Type must be a number between 1 and 7");
                
        }
        this.type_code = type;
    }

    public int Get_Type_Code() { return this.type_code; }
    public string Get_Type_Name() { return this.type_name; }
}
