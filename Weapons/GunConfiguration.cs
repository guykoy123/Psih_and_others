using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunConfiguration {

    static float[] RarityMultiplier = { 1f, 1.2f, 1.5f, 2f };

    static int[] PistolFireRateRange = { 150, 200 };
    static int[] PistolDamageRange = { 10, 50 };
    static int[] PistolMagazineRange = { 6, 15 };
    static float[] PistolAccuracyRange = { 0.88f, 0.97f };

    static int[] ShotgunFireRateRange = { 60, 120 };
    static int[] ShotgunDamageRange = { 100, 200 };
    static int[] ShotgunMagazineRange = { 1, 8 };
    static float[] ShotgunAccuracyRange = { 0.5f, 0.75f };

    static int[] SMGFireRateRange = { 400, 600 };
    static int[] SMGDamageRange = { 20, 60 };
    static int[] SMGMagazineRange = { 20, 50 };
    static float[] SMGAccuracyRange = { 0.8f, 0.9f };

    static int[] RifleFireRateRange = { 300, 500 };
    static int[] RifleDamageRange = { 40, 80 };
    static int[] RifleMagazineRange = { 20, 40 };
    static float[] RifleAccuracyRange = { 0.7f, 0.9f };

    static int[] LMGFireRateRange = { 200, 500 };
    static int[] LMGDamageRange = { 50, 100 };
    static int[] LMGMagazineRange = { 60, 100 };
    static float[] LMGAccuracyRange = { 0.6f, 0.8f };

    static int[] SniperFireRateRange = { 30, 100 };
    static int[] SniperDamageRange = { 100, 200 };
    static int[] SniperMagazineRange = { 1, 6 };
    static float[] SniperAccuracyRange = { 0.9f, 0.98f };

    static int[] RPGFireRateRange = { 30, 100 };
    static int[] RPGDamageRange = { 150, 300 };
    static int[] RPGMagazineRange = { 1, 5 };
    static float[] RPGAccuracyRange = { 0.8f, 0.95f };

    static int[] ZoomValue = { 5, 10, 15, 20, 30, 35 }; //will pick on randomly

    public static float[] GenerateStats(int type, int RarityCode)
    {
        float FireRate=0f;
        float Damage=0f;
        float MagazineSize=0f;
        float Accuracy = 0f; //not affected by rarity

        System.Random rnd = new System.Random();

        switch (type) //pistol - 1, shotgun - 2, smg - 3, assault rifle - 4, lmg - 5, sniper - 6, rpg - 7
        {
            case 1:
                FireRate = rnd.Next(PistolFireRateRange[0], PistolFireRateRange[1]) * RarityMultiplier[RarityCode];
                Damage = rnd.Next(PistolDamageRange[0], PistolDamageRange[1]) * RarityMultiplier[RarityCode];
                MagazineSize = rnd.Next(PistolMagazineRange[0], PistolMagazineRange[1]) * RarityMultiplier[RarityCode];
                Accuracy = Random.Range(PistolAccuracyRange[0], PistolAccuracyRange[1]);
                break;
            case 2:
                FireRate = rnd.Next(ShotgunFireRateRange[0], ShotgunFireRateRange[1]) * RarityMultiplier[RarityCode];
                Damage = rnd.Next(ShotgunDamageRange[0], ShotgunDamageRange[1]) * RarityMultiplier[RarityCode];
                MagazineSize = rnd.Next(ShotgunMagazineRange[0], ShotgunMagazineRange[1]) * RarityMultiplier[RarityCode];
                Accuracy = Random.Range(ShotgunAccuracyRange[0], ShotgunAccuracyRange[1]);
                break;
            case 3:
                FireRate = rnd.Next(SMGFireRateRange[0], SMGFireRateRange[1]) * RarityMultiplier[RarityCode];
                Damage = rnd.Next(SMGDamageRange[0], SMGDamageRange[1]) * RarityMultiplier[RarityCode];
                MagazineSize = rnd.Next(SMGMagazineRange[0], SMGMagazineRange[1]) * RarityMultiplier[RarityCode];
                Accuracy = Random.Range(SMGAccuracyRange[0], SMGAccuracyRange[1]);
                break;
            case 4:
                FireRate = rnd.Next(RifleFireRateRange[0], RifleFireRateRange[1]) * RarityMultiplier[RarityCode];
                Damage = rnd.Next(RifleDamageRange[0], RifleDamageRange[1]) * RarityMultiplier[RarityCode];
                MagazineSize = rnd.Next(RifleMagazineRange[0], RifleMagazineRange[1]) * RarityMultiplier[RarityCode];
                Accuracy = Random.Range(RifleAccuracyRange[0], RifleAccuracyRange[1]);
                break;
            case 5:
                FireRate = rnd.Next(LMGFireRateRange[0], LMGFireRateRange[1]) * RarityMultiplier[RarityCode];
                Damage = rnd.Next(LMGDamageRange[0], LMGDamageRange[1]) * RarityMultiplier[RarityCode];
                MagazineSize = rnd.Next(LMGMagazineRange[0], LMGMagazineRange[1]) * RarityMultiplier[RarityCode];
                Accuracy = Random.Range(LMGAccuracyRange[0], LMGAccuracyRange[1]);
                break;
            case 6:
                FireRate = rnd.Next(SniperFireRateRange[0], SniperFireRateRange[1]) * RarityMultiplier[RarityCode];
                Damage = rnd.Next(SniperDamageRange[0], SniperDamageRange[1]) * RarityMultiplier[RarityCode];
                MagazineSize = rnd.Next(SniperMagazineRange[0], SniperMagazineRange[1]) * RarityMultiplier[RarityCode];
                Accuracy = Random.Range(SniperAccuracyRange[0], SniperAccuracyRange[1]);
                break;
            case 7:
                FireRate = rnd.Next(RPGFireRateRange[0], RPGFireRateRange[1]) * RarityMultiplier[RarityCode];
                Damage = rnd.Next(RPGDamageRange[0], RPGDamageRange[1]) * RarityMultiplier[RarityCode];
                MagazineSize = rnd.Next(RPGMagazineRange[0], RPGMagazineRange[1]) * RarityMultiplier[RarityCode];
                Accuracy = Random.Range(RPGAccuracyRange[0], RPGAccuracyRange[1]);
                break;
        }

        int GunZoomValue = ZoomValue[rnd.Next(0, ZoomValue.Length)];

        return new float[] {FireRate,Damage,MagazineSize,GunZoomValue,Accuracy};
    }
}
