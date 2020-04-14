using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class Gun:Item
{
    GunType GunType;
    GameObject WeaponMesh;

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

    public Gun(int gunType,int rarityCode, int FireMode = 0)
    {
        //create base
        this.ItemIcon = Resources.Load<Sprite>("UI/Images/Inventory/Pistol Icon/TestIcon");
        this.ItemRarity = new Rarity(rarityCode);
        this.ItemType = new ItemType(1);

        //create GunType object and save it
        GunType = new GunType(gunType);

        //load randome weapon model
        LoadWeaponMesh();

        //check fire mode legal and save
        SetFiringMode(FireMode);

        //this.SetRarity(rarityCode);

        //randomize gun stats based on rarity and type
        float[] stats = GunConfiguration.GenerateStats(GunType.GetTypeCode(),rarityCode);

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

    private void LoadWeaponMesh()
    {
        //loads random weapon model based on the set weapon type
        var files = Directory.GetFiles(Directory.GetCurrentDirectory()+ "\\Assets\\Resources\\Prefabs\\Weapons\\"+ GunType.GetTypeName(), "*.prefab"); //get all model files from the gun type folder
        int index = new System.Random().Next(0, files.Length); //pick random file
        WeaponMesh = Resources.Load<GameObject>("Prefabs\\Weapons\\"+ GunType.GetTypeName()+"\\"+Path.GetFileNameWithoutExtension(files[index])); //load weapon model
    }

    public GameObject GetWeaponMesh() { return WeaponMesh;}
    public float GetFiringRate() { return FireRate; }
    public float GetDamage() { return Damage; }
    public int GetFiringMode() { return FiringMode; }
    public float GetZoomValue() { return ZoomValue; }
    public float GetAccuracy() { return Accuracy; }
    public int GetCurrentAmmo() { return CurrentAmmo; }
    public int GetMagazineSize() { return MagazineSize; }
    public GameObject GetMuzzleFlash() { return MuzzleFlash; }
    public int GetTypeCode() { return GunType.GetTypeCode(); }
    public GunType GetGunType() { return GunType; }

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
        text += this.ItemType.GetTypeName()+" Type: " + GunType.GetTypeName();
        text += ", Rarity: " + this.ItemRarity.GetRarityName();
        text += ", Damage: " + Damage.ToString();
        text += ", Fire Rate: " + FireRate.ToString();
        text += ",\r\n Firing mode: " + FiringMode;
        text += ",Zoom value: " + ZoomValue;
        text += ", Accuracy: " + Accuracy;
        text += ", Magazine size: " + MagazineSize;
        return text;
    }
}
