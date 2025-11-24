using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private GameObject playerUIPrefab;
    [SerializeField] private GameObject shopUIPrefab;

    private UIContoller uIContoller;
    private Shop shopUI;

    private Inventory inventory;
    private Movement movement;
    private Attack attack;
    private PlayerHealth health;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        GameObject playerUI = Instantiate(playerUIPrefab, transform);
        GameObject shopObj = Instantiate(shopUIPrefab, transform);

        uIContoller = playerUI.GetComponent<UIContoller>();

        inventory = GetComponent<Inventory>();
        movement = GetComponent<Movement>();
        attack = GetComponent<Attack>();

        health = GetComponent<PlayerHealth>();
        health.OnDie += OnPlayerDeath;

        shopUI = shopObj.GetComponent<Shop>();
        shopUI.SetPlayerInventoryReference(inventory);

    }
    // Start is called before the first frame update
    void Start()
    {

        if(uIContoller != null)
        {
            uIContoller.BindToPlayer(this);
        }
        if(shopUI != null)
        {
            shopUI.OnBoughtNewItem += AddItemToInventory;
            shopUI.OnPlay += OnReplay;
            //False until the player dies
            shopUI.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(shopUI);

    }

    public void CallDisplayShootingSide(float side)
    {
        uIContoller.DisplayShootSide(side);
    }

    //Inventory

    //Returns true with the change was successfull
    public bool CallChangeItemAmount(Item item, int amount)
    {
        return inventory.ChangeAmountOfItem(item, amount);
    }

    public int CallGetItemAmount(Item item)
    {
        return inventory.GetItemAmount(item);
    }

    public void AddItemToInventory(Item item, int amount)
    {
        inventory.InsertNewItem(item, amount);
    }

    public void OnPlayerDeath()
    {

        uIContoller.gameObject.SetActive(false);
        shopUI.gameObject.SetActive(true);
        movement.enabled = false;
        attack.enabled = false;

    }
    public void OnReplay()
    {
        GameManager.instance.OnReplay();
        shopUI.gameObject.SetActive(false);
        uIContoller.gameObject.SetActive(true);
        uIContoller.enabled = true;
        movement.enabled = true;
        attack.enabled = true;
    }

}
