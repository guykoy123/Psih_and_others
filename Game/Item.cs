using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    protected Sprite ItemIcon { get; set; }
    protected Rarity ItemRarity { get; set; }
    protected ItemType ItemType { get; set; }

    public Item()
    {
    }
    public Item(int rarityCode, int typeCode, Sprite icon = null)
    {
        ItemIcon = icon;
        ItemRarity = new Rarity(rarityCode);
        ItemType = new ItemType(typeCode);
    }
    public Sprite GetItemIcon()
    {
        return ItemIcon;
    }
}
