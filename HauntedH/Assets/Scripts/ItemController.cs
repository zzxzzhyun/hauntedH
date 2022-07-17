using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public Item item;

    void AddItem(Item newItem)
    {
        item = newItem;
    }

    void UseItem()
    {
        /*
        switch(item.ItemType)
        {
            case Item.ItemType.Flashlight:
                //
                break;
            case Item.ItemType.GhostCam:
                break;
            
            case Item.ItemType.Corn:
                break;
        }*/
    } 

}
