using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemController : MonoBehaviour
{
    public static InventoryItemController Instance;

    public Item item;

    private void Awake()
    {
        Instance = this;
    }


    public void AddItem(Item newItem)
    {
        item = newItem;

    }


    public void RemoveItem()
    {
        InventoryManager.Instance.Remove(item);
        Destroy(gameObject);
    }

    public void UseItem()
    {
        switch(item.itemType)
        {
            case Item.ItemType.Flashlight:
                SC_HumanController.Instance.ShowFlashlight();
                break;
            
            case Item.ItemType.GhostCam:
                SC_HumanController.Instance.DetectPhantom();
                break;
            
            case Item.ItemType.Corn:
                SC_HumanController.Instance.MakeKey();
                break;
            
            case Item.ItemType.Body:
                PhantomController.Instance.ShowBody(item);
                break;

            case Item.ItemType.Bat:
                PhantomController.Instance.HitCorn();
                break;

        }
    } 
}
