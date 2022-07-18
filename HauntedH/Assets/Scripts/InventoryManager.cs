using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<Item> Items = new List<Item>();
   
    public Transform ItemContent;
    public GameObject InventoryItem;

    public InventoryItemController InventoryItems;

    private void Awake()
    {
        Instance = this;
    }
   
    public void Add(Item item)
    {
        Items.Add(item);
    }

    public void Remove(Item item)
    {
        Items.Remove(item);
    }

    public void ListItems()
    {
        foreach (Transform item in ItemContent)
        {
            Destroy(item.gameObject);
        }

        foreach (var item in Items)
        {
            GameObject obj = Instantiate(InventoryItem, ItemContent);

            var itemName = obj.transform.Find("ItemName").GetComponent<TMPro.TextMeshProUGUI>();
            var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
            
            itemName.text = item.itemName;
            itemIcon.sprite = item.icon;
        }

        SetInventoryItems();

    }

    public void SetInventoryItems()
    {
        InventoryItems = ItemContent.GetComponentInChildren<InventoryItemController>();

        for (int i=0; i<Items.Count; i++)
        {
            InventoryItems.AddItem(Items[i]);
        }

    }

}