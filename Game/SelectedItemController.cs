using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SelectedItemController : MonoBehaviour
{
    Item item;
    InventorySlot PreviousSlot; //the previous slot of the item (to return when deselected)
    Image spriteImage; //image component of the object

    // Start is called before the first frame update
    void Start()
    {
        spriteImage = gameObject.GetComponent<Image>(); //get image component
        spriteImage.color = Color.clear; //make it invisible
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Input.mousePosition; //follow mouse position
    }

    public void SetItem(Item item, InventorySlot slot)
    {
        this.item = item;
        PreviousSlot = slot;
        spriteImage.sprite = item.GetItemIcon(); //update icon
        spriteImage.color = Color.white; //make image visible
    }

    public Item PopItem()
    {
        Item temp = item;

        //remove previous item data
        PreviousSlot = null;
        item = null;

        //make image invisible
        spriteImage.color = Color.clear;

        //return the item
        return temp;
    }

    public Item GetItem() { return item; }

    public InventorySlot GetPreviousSlot() { return PreviousSlot; }

    public void RemoveItem()
    {
        //remove item data
        item = null;
        PreviousSlot = null;

        //make image invisible
        spriteImage.color = Color.clear;
    }

    public bool IsEmtpy() { return (item == null); }
}
