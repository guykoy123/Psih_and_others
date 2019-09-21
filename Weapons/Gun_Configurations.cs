using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Configurations {

    static float[] Rarity_Multiplier = { 1f, 1.2f, 1.5f, 2f };

    static int[] Pistol_Fire_Rate_Range = { 150, 200 };
    static int[] Pistol_Damage_Range = { 10, 50 };
    static int[] Pistol_Magazine_Range = { 6, 15 };

    static int[] Shotgun_Fire_Rate_Range = { 60, 120 };
    static int[] Shotgun_Damage_Range = { 100, 200 };
    static int[] Shotgun_Magazine_Range = { 1, 8 };

    static int[] SMG_Fire_Rate_Range = { 400, 600 };
    static int[] SMG_Damage_Range = { 20, 60 };
    static int[] SMG_Magazine_Range = { 20, 50 };

    static int[] Rifle_Fire_Rate_Range = { 300, 500 };
    static int[] Rifle_Damage_Range = { 40, 80 };
    static int[] Rifle_Magazine_Range = { 20, 40 };

    static int[] LMG_Fire_Rate_Range = { 200, 500 };
    static int[] LMG_Damage_Range = { 50, 100 };
    static int[] LMG_Magazine_Range = { 60, 100 };

    static int[] Sniper_Fire_Rate_Range = { 30, 100 };
    static int[] Sniper_Damage_Range = { 100, 200 };
    static int[] Sniper_Magazine_Range = { 1, 6 };

    static int[] RPG_Fire_Rate_Range = { 30, 100 };
    static int[] RPG_Damage_Range = { 150, 300 };
    static int[] RPG_Magazine_Range = { 1, 5 };

    static int[] Zoom_Value = { 5, 10, 15, 20, 30, 35 }; //will pick on randomly

    public static float[] Generate_Stats(int type, int rarity_code)
    {
        float fire_rate=0f;
        float damage=0f;
        float magazine_size=0f;

        System.Random rnd = new System.Random();

        switch (type) //pistol - 1, shotgun - 2, smg - 3, assault rifle - 4, lmg - 5, sniper - 6, rpg - 7
        {
            case 1:
                fire_rate = rnd.Next(Pistol_Fire_Rate_Range[0], Pistol_Fire_Rate_Range[1]) * Rarity_Multiplier[rarity_code];
                damage = rnd.Next(Pistol_Damage_Range[0], Pistol_Damage_Range[1]) * Rarity_Multiplier[rarity_code];
                magazine_size = rnd.Next(Pistol_Magazine_Range[0], Pistol_Magazine_Range[1]) * Rarity_Multiplier[rarity_code];
                break;
            case 2:
                fire_rate = rnd.Next(Shotgun_Fire_Rate_Range[0], Shotgun_Fire_Rate_Range[1]) * Rarity_Multiplier[rarity_code];
                damage = rnd.Next(Shotgun_Damage_Range[0], Shotgun_Damage_Range[1]) * Rarity_Multiplier[rarity_code];
                magazine_size = rnd.Next(Shotgun_Magazine_Range[0], Shotgun_Magazine_Range[1]) * Rarity_Multiplier[rarity_code];
                break;
            case 3:
                fire_rate = rnd.Next(SMG_Fire_Rate_Range[0], SMG_Fire_Rate_Range[1]) * Rarity_Multiplier[rarity_code];
                damage = rnd.Next(SMG_Damage_Range[0], SMG_Damage_Range[1]) * Rarity_Multiplier[rarity_code];
                magazine_size = rnd.Next(SMG_Magazine_Range[0], SMG_Magazine_Range[1]) * Rarity_Multiplier[rarity_code];
                break;
            case 4:
                fire_rate = rnd.Next(Rifle_Fire_Rate_Range[0], Rifle_Fire_Rate_Range[1]) * Rarity_Multiplier[rarity_code];
                damage = rnd.Next(Rifle_Damage_Range[0], Rifle_Damage_Range[1]) * Rarity_Multiplier[rarity_code];
                magazine_size = rnd.Next(Rifle_Magazine_Range[0], Rifle_Magazine_Range[1]) * Rarity_Multiplier[rarity_code];
                break;
            case 5:
                fire_rate = rnd.Next(LMG_Fire_Rate_Range[0], LMG_Fire_Rate_Range[1]) * Rarity_Multiplier[rarity_code];
                damage = rnd.Next(LMG_Damage_Range[0], LMG_Damage_Range[1]) * Rarity_Multiplier[rarity_code];
                magazine_size = rnd.Next(LMG_Magazine_Range[0], LMG_Magazine_Range[1]) * Rarity_Multiplier[rarity_code];
                break;
            case 6:
                fire_rate = rnd.Next(Sniper_Fire_Rate_Range[0], Sniper_Fire_Rate_Range[1]) * Rarity_Multiplier[rarity_code];
                damage = rnd.Next(Sniper_Damage_Range[0], Sniper_Damage_Range[1]) * Rarity_Multiplier[rarity_code];
                magazine_size = rnd.Next(Sniper_Magazine_Range[0], Sniper_Magazine_Range[1]) * Rarity_Multiplier[rarity_code];
                break;
            case 7:
                fire_rate = rnd.Next(RPG_Fire_Rate_Range[0], RPG_Fire_Rate_Range[1]) * Rarity_Multiplier[rarity_code];
                damage = rnd.Next(RPG_Damage_Range[0], RPG_Damage_Range[1]) * Rarity_Multiplier[rarity_code];
                magazine_size = rnd.Next(RPG_Magazine_Range[0], RPG_Magazine_Range[1]) * Rarity_Multiplier[rarity_code];
                break;
        }

        int zoom_value = Zoom_Value[rnd.Next(0, Zoom_Value.Length)];

        return new float[] {fire_rate,damage,magazine_size,zoom_value};
    }
}
