using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemType 
{
    /*
     * stores the item types
     * item codes:
     *      1 - weapon
     *      TODO: add the rest
     */
    int TypeCode;
    string TypeName;

    public ItemType(int typeCode)
    {
        switch (typeCode)
        {
            case 1:
                TypeCode = typeCode;
                TypeName = "Weapon";
                break;
            default:
                throw new System.ArgumentOutOfRangeException("typeCode", "Type code is out of the set range");
        }
    }

    public string GetTypeName()
    {
        return TypeName;
    }
}
