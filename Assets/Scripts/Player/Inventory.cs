using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    private PlayerController playerController;
    private Dictionary<string, int> items = new Dictionary<string, int>();

    void Awake()
    {
        playerController = GetComponent<PlayerController>();

        items.Add("log", 0);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        

        foreach (var item in items)
        {
            playerController.CallDisplayItemAmount(item.Value, item.Key);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            AddToItem(1, "log");
        }
    }

    public void AddToItem(int amount, string type)
    {
        if (items.ContainsKey(type))
        {
            items[type] += amount;
            playerController.CallDisplayItemAmount(items[type], type);

        }
        else
        {
            Debug.Log("Could not find specified item: " + type);
        }
    }


    public int GetItemAmount(string type)
    {
        if(items.ContainsKey(type))
        {
            return items[type];
        }
        else
        {
            Debug.Log("Could not find specified item: " + type);
            return 1;
        }
    }
    public bool DecreaseItemAmount(int amount, string type)
    {
        if (items.ContainsKey(type))
        {
            if (items[type] - amount < 0)
            {
                //Display message
                return false;
            }
            else
            {
                items[type] -= amount;
                playerController.CallDisplayItemAmount(items[type], type);
                return true;
            }
                
        }
        else
        {
            Debug.Log("Could not find specified item: " + type);
            return false;
        }
    }

}
