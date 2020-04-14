using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    int EquippedGunIndex; //index of gun currently in player hands
    Gun[] GunArray = new Gun[4]; //create 4 slots for quickly swaping guns
    List<InventorySlot> InventorySlots = new List<InventorySlot>();

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
        GameObject[] slots = GameObject.FindGameObjectsWithTag("InventorySlot");
        Debug.Log("slots:" + slots.Length);
        Debug.Log(slots[0].name);
        foreach(GameObject i in slots)
        {
            InventorySlots.Add(i.GetComponent<InventorySlot>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Open)
        {
            UpdateHotkeyIcons();
            if(GunArray[2]!= null)
            {
                InventorySlots[0].SetItem(GunArray[2]);
                GunArray[2] = null;
            }
                

        }
    }

    private void UpdateHotkeyIcons()
    {
        for(int i = 0; i < GunArray.Length; i++)
        {
            if(GunArray[i] != null)
                GameObject.Find("GunSlot"+(i+1)).GetComponent<Image>().sprite = GunArray[i].GetItemIcon();
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
        if (i >= GunArray.Length)
            return GetNextGunIndex(0);
        else if (GunArray[i] != null)
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
        else if (GunArray[i] != null)
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
        if (i>=0 && i < GunArray.Length)
            if(GunArray[i] != null)
                EquippedGunIndex = i;

        weaponController.EquipGun(GunArray[EquippedGunIndex]);
    }

    public void SetGunInIndex(Gun gun, int i)
    {
        if (i >= 0 && i < GunArray.Length)
            GunArray[i] = gun;
    }

    public void RemoveGunInIndex(int i)
    {
        if (GunArray[i] != null)
            GunArray[i] = null;
    }

    public void Toggle()
    {
        //Toggle inventory open/close
        if (Open)
        {
            SetVisible(false);
            Open = false;
            gameManager.ClosedMenu();
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
