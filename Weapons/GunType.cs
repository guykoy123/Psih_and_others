using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunType
{
    string TypeName = ""; //can be: pistol - 1, shotgun - 2, smg - 3, assault rifle - 4, lmg - 5, sniper - 6, rpg - 7
    int type_code = 0;

    public GunType(int type)
    {
        /* 
        in:  gun type (by number) and firing mode (defaul is auto and semi)
        gun types: pistol - 1, shotgun - 2, smg - 3, assault rifle - 4, lmg - 5, sniper - 6, rpg - 7
        */

        SetGunType(type); //save gun type if lega;

    }

    private void SetGunType(int type)
    {
        //checks that given type is legal and saves it
        //if type is not legal throws an exception
        switch (type)
        {
            case 1:
                this.TypeName = "Pistol";
                break;
            case 2:
                this.TypeName = "Shotgun";
                break;
            case 3:
                this.TypeName = "SMG";
                break;
            case 4:
                this.TypeName = "Assault Rifle";
                break;
            case 5:
                this.TypeName = "LMG";
                break;
            case 6:
                this.TypeName = "Sniper";
                break;
            case 7:
                this.TypeName = "RPG";
                break;
            default:
                throw new System.ArgumentOutOfRangeException("type","Type must be a number between 1 and 7");
                
        }
        this.type_code = type;
    }

    public int GetTypeCode() { return this.type_code; }
    public string GetTypeName() { return this.TypeName; }
}
