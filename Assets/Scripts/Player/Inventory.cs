using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    private PlayerController playerController;

    [SerializeField] private List<Item> itemAssigner = new List<Item>();
    [SerializeField] private Dictionary<Item, int> items = new Dictionary<Item, int>();

    void Awake()
    {
        playerController = GetComponent<PlayerController>();

        foreach (var item in itemAssigner)
        {
            if (item.isProjectile)
            {
                items.Add(item, item.maxStack);
            }
            else
            {
                items.Add(item, 0);
            }

            if (item.isInUI)
            {
                playerController.CallDisplayItemAmount(items[item], item);
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

    public void AddToItem(int amount, Item item)
    {
        if (items.ContainsKey(item))
        {
            items[item] += amount;
            playerController.CallDisplayItemAmount(items[item], item);

        }
        else
        {
            Debug.Log("Could not find specified item: " + item.itemName);
        }
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
    public bool DecreaseItemAmount(int amount, Item item)
    {
        if (items.ContainsKey(item))
        {
            if (items[item] - amount < 0)
            {
                //Display message
                return false;
            }
            else
            {
                items[item] -= amount;
                playerController.CallDisplayItemAmount(items[item], item);
                return true;
            }
                
        }
        else
        {
            Debug.Log("Could not find specified item: " + item.itemName);
            return false;
        }
    }

}
