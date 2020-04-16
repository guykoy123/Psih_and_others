using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSlot : InventorySlot
{
    //create dropdown menu in the editor to select type of equipment in the slot (weapon,clothes)
    public enum EquipmentType { Weapon,Other};
    public EquipmentType Type;

    // Start is called before the first frame update
    void Start()
    {
        base.Setup();
        switch (Type)
        {
            case EquipmentType.Weapon:
                EmptySlotSprite = Resources.Load<Sprite>("UI/Images/Inventory/Gun Slot/GunSlot");
                RemoveItem(); //call function to reload new empty slot sprite
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void SetItem(Item item)
    {
        base.SetItem(item);
        Debug.Log("set weapon in slot");
    }

}
