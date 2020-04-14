using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    Item Item; //the object stored in this slot
    Button btn;
    InventoryController Inventory;
    SelectedItemController SelectedItem;
    Sprite EmptySlotSprite;

    // Start is called before the first frame update
    void Start()
    {
        Inventory = gameObject.GetComponentInParent<InventoryController>();
        btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(Click);
        SelectedItem = GameObject.Find("SelectedItem").GetComponent<SelectedItemController>();
        EmptySlotSprite = Resources.Load<Sprite>("UI/Images/Inventory/Empty Slot/EmptySlot");
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetItem(Item item)
    {
        Item = item;
        gameObject.GetComponent<Image>().sprite = Item.GetItemIcon();
    }
    public void RemoveItem()
    {
        if (!IsEmpty())
            Item = null;
        gameObject.GetComponent<Image>().sprite = EmptySlotSprite;
    }

    public Item GetItem()
    {
        return Item;
    }

    public bool IsEmpty()
    {
        if (Item != null)
            return false;
        return true;
    }

    public void Click()
    {
        if (!IsEmpty())
        {
            SelectedItem.SetItem(Item,this.gameObject);
            RemoveItem();
        }
        else
        {
            if (!SelectedItem.IsEmtpy())
            {
                SetItem(SelectedItem.RemoveItem());
            }
            
        }
    }


}
