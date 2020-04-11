using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        //make invisble
        SetVisible(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Open()
    {
        SetVisible(true);
    }

    public void Close()
    {
        SetVisible(false);
    }

    private void SetVisible(bool visible)
    {
        //make visible/invisible by enableing/disabling all image components of children
        Image[] images = GetComponentsInChildren<Image>();
        foreach (Image i in images)
            i.enabled = visible;
    }
}
