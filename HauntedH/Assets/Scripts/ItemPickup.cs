using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;

    void Pickup()
    {
        InventoryManager inventoryManager;
        switch (item.itemType)
        {
            case Item.ItemType.Flashlight:
                inventoryManager = GameObject.Find("HumanPlayer").GetComponentInChildren<InventoryManager>();
                inventoryManager.Add(item);
                Destroy(gameObject);
                inventoryManager.ListItems();
                break;

            case Item.ItemType.GhostCam:
                inventoryManager = GameObject.Find("HumanPlayer").GetComponentInChildren<InventoryManager>();
                inventoryManager.Add(item);
                Destroy(gameObject);
                inventoryManager.ListItems();
                break;

            case Item.ItemType.Corn:
                inventoryManager = GameObject.Find("HumanPlayer").GetComponentInChildren<InventoryManager>();
                inventoryManager.Add(item);
                Destroy(gameObject);
                inventoryManager.ListItems();
                break;

            case Item.ItemType.Body:
                inventoryManager = GameObject.Find("PhantomPlayer").GetComponentInChildren<InventoryManager>();
                inventoryManager.Add(item);
                Destroy(gameObject);
                inventoryManager.ListItems();
                break;

            case Item.ItemType.Bat:
                inventoryManager = GameObject.Find("PhantomPlayer").GetComponentInChildren<InventoryManager>();
                inventoryManager.Add(item);
                Destroy(gameObject);
                inventoryManager.ListItems();
                break;

        }


    }

    private void OnMouseDown()
    {
        Pickup();
    }
}