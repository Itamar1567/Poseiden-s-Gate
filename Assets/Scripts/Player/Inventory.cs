using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public event Action<string> OnPrompt;
    public event Action<Item, int> OnInventoryChange;

    [SerializeField] private List<Item> itemAssigner = new List<Item>();
    [SerializeField] private Dictionary<Item, int> items = new Dictionary<Item, int>();

    void Awake()
    {
        foreach (var item in itemAssigner)
        {
            items.Add(item, 0);
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

    public void SetUsedOneTimePurchaseStatus(Item item, bool setTo)
    {
        foreach (var pair in items)
        {
            if (pair.Key == item)
            {
                pair.Key.usedOneTimePurchase = setTo;
            }
        }
    }

    public bool IsUsedOneTimePurchase(Item item)
    {
        foreach(var pair in items)
        {
            if (pair.Key == item)
            {
                return pair.Key.usedOneTimePurchase;
            }            
        }

        Debug.Log("Could not find item in the inventory");
        return false;

    }
    public bool HasItem(Item item)
    {
        return items.ContainsKey(item);
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
                OnPrompt.Invoke(item.name + " Filled");
                items[item] = item.maxStack;
            }
            else
            {
                items[item] += amount;
            }
                
            OnInventoryChange?.Invoke(item, items[item]);
            return true;

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
                if (items[item] + amount > item.maxStack)
                {
                    Debug.Log("Added too much");
                    items[item] = item.maxStack;
                }
                else { items[item] += amount; }
                    
            }
            else
            {
                items.Add(item, amount);
            }

            OnInventoryChange.Invoke(item, items[item]);

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
