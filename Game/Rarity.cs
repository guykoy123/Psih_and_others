using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rarity
{
    /*
     * holds the rarity level of an item
     * Rarity codes:
     *      1 - Common
     *      2 - Uncommon
     *      3 - Rare
     *      4 - Legendary
     */

    string RarityName;
    int RarityCode;

    public Rarity(int rarityCode)
    {
        switch (rarityCode)
        {
            case 1:
                RarityCode = rarityCode;
                RarityName = "Common";
                break;
            case 2:
                RarityCode = rarityCode;
                RarityName = "Uncommon";
                break;
            case 3:
                RarityCode = rarityCode;
                RarityName = "Rare";
                break;
            case 4:
                RarityCode = rarityCode;
                RarityName = "Legendary";
                break;
            default:
                throw new System.ArgumentOutOfRangeException("rarityCode", "Rariry code must be between 1 and 4");
        }
    }

    public int GetRarityCode()
    {
        return RarityCode;
    }

    public string GetRarityName()
    {
        return RarityName;
    }
}
