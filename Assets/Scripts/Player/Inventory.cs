using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public event Action<string> OnPrompt;
    public event Action<Item, int> OnInventoryChange;


    private PlayerController playerController;

    [SerializeField] private List<Item> itemAssigner = new List<Item>();
    [SerializeField] private Dictionary<Item, int> items = new Dictionary<Item, int>();

    void Awake()
    {
        playerController = GetComponent<PlayerController>();

        foreach (var item in itemAssigner)
        {
            switch (item.categories)
            {
                case ItemCategories.Projectile: 
                    items.Add(item, item.maxStack);
                    break;

                default: items.Add(item, 0);
                    break;
            }
        }

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        

           
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public int GetItemAmount(Item item)
    {
        if(items.ContainsKey(item))
        {
            return items[item];
        }
        else
        {
            Debug.Log("Could not find specified item: " + item);
            return 1;
        }
    }

    //Add or decrease from a specified item
    public bool ChangeAmountOfItem(Item item, int amount)
    {
        if (items.ContainsKey(item))
        {
            if (items[item] + amount < 0)
            {
                OnPrompt.Invoke("Missing required resources");
                return false;
            }
            if (items[item] + amount > item.maxStack)
            {
                OnPrompt.Invoke(item.name + " Full");
                return false;
            }
            else
            {
                items[item] += amount;
                OnInventoryChange.Invoke(item, items[item]);
                return true;
            }
                
        }
        else
        {
            Debug.Log("Could not find specified item: " + item.itemName);
            return false;
        }
    }

    public bool InsertNewItem(Item item, int amount)
    {
        try
        {
            if (items.ContainsKey(item))
            {
                items[item] = amount;
            }
            else
            {
                items.Add(item, amount);
            }

            return true;
        }
        catch (Exception e)
        {
            Debug.Log(e);
            return false;
        }
        
    }

    public Dictionary<Item, int> GetInventory()
    {
        return items;
    }

}
