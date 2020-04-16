using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    int EquippedGunIndex; //index of gun currently in player hands
    List<EquipmentSlot> GunArray = new List<EquipmentSlot>(); //create slots for quickly swaping guns
    List<InventorySlot> InventorySlots = new List<InventorySlot>();
    List<EquipmentSlot> EquipmentSlots = new List<EquipmentSlot>();

    bool Open = false; //the current state of the invetory (open-true/closed-false)

    GameManager gameManager;
    WeaponController weaponController;

    bool MouseDown = false;
    // Start is called before the first frame update
    void Start()
    {
        //make invisble
        SetVisible(false);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        weaponController = GameObject.Find("Weapon").GetComponent<WeaponController>();

        //setup inventory slots list
        GameObject[] slots = GameObject.FindGameObjectsWithTag("InventorySlot");
        Debug.Log("slots:" + slots.Length);
        foreach(GameObject i in slots)
        {
            InventorySlots.Add(i.GetComponent<InventorySlot>());
        }

        //setup equipment slots list and gun array
        GameObject[] temp = GameObject.FindGameObjectsWithTag("EquipmentSlot");
        Debug.Log("equipment slots:" + slots.Length);
        foreach (GameObject i in temp)
        {
            EquipmentSlot eqSlot = i.GetComponent<EquipmentSlot>();
            EquipmentSlots.Add(eqSlot);
            if (eqSlot.Type == EquipmentSlot.EquipmentType.Weapon)
                GunArray.Add(eqSlot);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Open)
        {
                

        }
    }


    public void EquipNextGun()
    {
        //run recursibve funcrion that rerievs index of the next gun in the array
        EquipGunInIndex(GetNextGunIndex(EquippedGunIndex + 1));
    }

    private int GetNextGunIndex(int i)
    {
        /*
         * recursive function
         * looks for the next gun in the gun array
         */
        if (i >= GunArray.Count)
            return GetNextGunIndex(0);
        else if (!GunArray[i].IsEmpty())
            return i;
        return GetNextGunIndex(i + 1);
    }

    public void EquipPreviousGun()
    {
        EquipGunInIndex(GetPreviousGunIndex(EquippedGunIndex - 1));
    }

    private int GetPreviousGunIndex(int i)
    {
        /*
         * recursive function
         * looks for the previous gun in the gun array
         */
        if (i < 0)
            return GetPreviousGunIndex(3);
        else if (!GunArray[i].IsEmpty())
            return i;
        return GetPreviousGunIndex(i - 1);
    }

    public void EquipGunInIndex(int i)
    {
        /*
         * if index within range check that not empty equip requested gun 
         * if not do no equip different gun
         * return equipped gun
         */
        if (i>=0 && i < GunArray.Count)
            if(!GunArray[i].IsEmpty())
                EquippedGunIndex = i;

        weaponController.EquipGun((Gun)GunArray[EquippedGunIndex].GetItem());
    }

    public void SetGunInIndex(Gun gun, int i)
    {
        if (i >= 0 && i < GunArray.Count)
            GunArray[i].SetItem( gun);
    }
    
    public void RemoveGunInIndex(int i)
    {
        if (!GunArray[i].IsEmpty())
            GunArray[i] = null;
    }
    void UpdateEquipment()
    {
        /*
         * updates player based on the changes made to the equipment
         */
        if (GunArray[EquippedGunIndex].IsEmpty())
        {
            weaponController.RemoveGun();
        }
        weaponController.EquipGun((Gun)GunArray[EquippedGunIndex].GetItem());
    }
    public void Toggle()
    {
        //Toggle inventory open/close
        if (Open)
        {
            SetVisible(false);
            Open = false;
            gameManager.ClosedMenu();
            UpdateEquipment();
        }
        else
        {
            SetVisible(true);
            Open = true;
            gameManager.OpenedMenu();
        }
    }

    private void SetVisible(bool visible)
    {
        //make visible/invisible by enableing/disabling all image/text components of children
        Image[] images = GetComponentsInChildren<Image>();
        foreach (Image i in images)
            i.enabled = visible;
        Text[] texts = GetComponentsInChildren<Text>();
        foreach (Text t in texts)
            t.enabled = visible;
    }
}
