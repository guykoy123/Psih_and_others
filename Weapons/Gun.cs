using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun
{
    //pistol - 1, shotgun - 2, smg - 3, assault rifle - 4, lmg - 5, sniper - 6, rpg - 7

    //rarity codes: 0 - common (default), 1 - uncommon, 2 - rare, 3 - legendary
    int RarityCode;
    string RarityName;

    GunTypes Type;

    //firing mode: 0 - auto, 1 - semi
    int FiringMode = 0; 

    float FireRate;
    float Damage;
    int MagazineSize;
    float ZoomValue;
    float Accuracy;

    int CurrentAmmo;

    //TODO: load object from source not manualy
    public GameObject MuzzleFlash;

    public Gun(int type,int rarity = 0, int FireMode = 0)
    {
        //create GunTypes object
        Type = new GunTypes(type);

        //check fire mode legal and save
        SetFiringMode(FireMode);

        //check rarity and save
        SetRarityName(rarity);

        //randomize gun stats based on rarity and type
        float[] stats = GunConfiguration.GenerateStats(Type.GetTypeCode(),RarityCode);
        Debug.Log(stats[4]);
        //save stats
        FireRate = stats[0];
        Damage = stats[1];
        MagazineSize = (int)stats[2];
        ZoomValue = stats[3];
        Accuracy = stats[4];

        CurrentAmmo = MagazineSize; //reset magazine ammo
    }

    private void SetFiringMode(int FireMode)
    {
        if (FireMode > 1 || FireMode < 0) //check if fire mode out of range
        {
            throw new System.ArgumentOutOfRangeException("FireMode", "Firing mode must be 0 or 1"); // throw exception
        }
        else
            FiringMode = FireMode; //save fire mode
    }

    private void SetRarityName(int rarity)
    {
        //checks if rarity code is legal and updates RarityName
        switch (rarity)
        {
            case 0:
                RarityName = "Common";
                break;
            case 1:
                RarityName = "Uncommon";
                break;
            case 2:
                RarityName = "Rare";
                break;
            case 3:
                RarityName = "Legendary";
                break;
            default:
                throw new System.ArgumentOutOfRangeException("rarity", "Must be a value between 0 and 3");
        }
        this.RarityCode = rarity;
    }


    public float GetFiringRate() { return FireRate; }
    public float GetDamage() { return Damage; }
    public string Get_Rarity() { return RarityName; }
    public int GetFiringMode() { return FiringMode; }
    public float GetZoomValue() { return ZoomValue; }
    public float GetAccuracy() { return Accuracy; }
    public int GetCurrentAmmo() { return CurrentAmmo; }
    public int GetMagazineSize() { return MagazineSize; }
    public GameObject Get_MuzzleFlash() { return MuzzleFlash; }

    public bool Shoot()
    {
        //check if the magazine is not empty
        //if not empty, reduces by 1 and returns true
        //if empty returns false

        if (CurrentAmmo > 0) //cehck that magazine is not empty
        {
            CurrentAmmo -= 1; //remove one bullet
            return true; //return true (means player can shoot)
        }
        return false; //return false because magazine is empty and player can't shoot
    }

    public void Reload()
    {
        //resets current ammo
        CurrentAmmo = MagazineSize;
    }

    public override string ToString()
    {
        string text = "";
        text += "Type: " + Type.GetTypeName();
        text += ", Rarity: " + RarityName;
        text += ", Damage: " + Damage.ToString();
        text += ", Fire Rate: " + FireRate.ToString();
        text += ", Firing mode: " + FiringMode;
        text += ",Zoom value: " + ZoomValue;
        return text;
    }
}
