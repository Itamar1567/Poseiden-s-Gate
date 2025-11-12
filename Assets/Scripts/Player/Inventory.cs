using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    private PlayerController playerController;
    private Dictionary<string, int> items = new Dictionary<string, int>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        items.Add("log", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            AddToItem(1, "log");
        }
    }

    public void AddToItem(int amount, string type)
    {
        if(items.ContainsKey(type))
        {
            items[type] += amount;
            playerController.CallDisplayItemAmount(items[type], type);

        }
        else
        {
            Debug.Log("Could not find specified item: " + type);
        }
    }
}
