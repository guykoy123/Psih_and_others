using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    protected Item Item { get; set; } //the object stored in this slot
    protected Button btn { get; set; } //button component of the slot
    protected InventoryController Inventory { get; set; }
    protected SelectedItemController SelectedItem { get; set; }
    protected Sprite EmptySlotSprite { get; set; } //the sprite displayed when slot is empty

    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }
    protected void Setup()
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

    public virtual void SetItem(Item item)
    { 
        Item = item;
        gameObject.GetComponent<Image>().sprite = item.GetItemIcon();
    }

    public virtual void RemoveItem()
    {
        if (!IsEmpty())
            Item = null;
        gameObject.GetComponent<Image>().sprite = EmptySlotSprite;
    }

    public Item GetItem()
    {
        return Item;
    }

    public bool IsEmpty() { return (Item == null); }

    public void Click()
    {
        if (!IsEmpty())
        {
            //this not empty and selected item not empty - perform switch between this item and the other one
            if (!SelectedItem.IsEmtpy())
            {
                //move this item to other location
                InventorySlot OtherSlot = SelectedItem.GetPreviousSlot();
                OtherSlot.SetItem(SelectedItem.GetItem());

                //put new item here
                SetItem(SelectedItem.PopItem());
            }
            //this not empty selected item empty - move the item here to selected item
            else
            {
                SelectedItem.SetItem(Item, this);
                RemoveItem();
            }
            
        }
        //this is empty - pop item from selected item here
        else
            SetItem(SelectedItem.PopItem());

    }


}
