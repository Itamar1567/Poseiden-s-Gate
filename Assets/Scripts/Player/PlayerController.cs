using NUnit.Framework.Constraints;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField] InputAction pauseMenuControl;

    [SerializeField] private Item coin;

    [SerializeField] private GameObject playerUIPrefab;
    [SerializeField] private GameObject shopUIPrefab;
    [SerializeField] private GameObject pauseMenuPrefab;

    private UIContoller uIContoller;
    private Shop shopUI;
    private PauseMenu pauseMenuUI;

    private Inventory inventory;
    private Movement movement;
    private Attack attack;
    private PlayerHealth health;

    private void Awake()
    {

        GameObject playerUI = Instantiate(playerUIPrefab);
        GameObject shopObj = Instantiate(shopUIPrefab);
        GameObject pauseMenu = Instantiate(pauseMenuPrefab);



        uIContoller = playerUI.GetComponent<UIContoller>();

        inventory = GetComponent<Inventory>();
        movement = GetComponent<Movement>();
        attack = GetComponent<Attack>();

        health = GetComponent<PlayerHealth>();
        health.OnDie += OnPlayerDeath;

        shopUI = shopObj.GetComponent<Shop>();
        shopUI.SetPlayerInventoryReference(inventory);

        pauseMenuUI = pauseMenu.GetComponent<PauseMenu>();

    }
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.OnRoundChanged += OnRoundChangedAddCoins;
        }
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
        if (pauseMenuControl.WasPressedThisFrame())
        {
            if(pauseMenuUI.gameObject.activeSelf == true)
            {
                pauseMenuUI.gameObject.SetActive(false);
                Time.timeScale = 1.0f;
            }
            else
            {
                pauseMenuUI.gameObject.SetActive(true);
                Time.timeScale = 0f;
            }
        }
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

    public void OnRoundChangedAddCoins(int round)
    {
        CallChangeItemAmount(coin, round * 10);
    }

    public void MaxAmmo()
    {
        uIContoller.SetPrompt("Max Ammo");
        attack.MaxAmmo();
    }

    public int CallGetItemAmount(Item item)
    {
        return inventory.GetItemAmount(item);
    }

    public void AddItemToInventory(Item item, int amount)
    {
        switch (item.categories)
        {
            case ItemCategories.Projectile:
                attack.AddNewProjectile(item);
                break;
        }

        inventory.InsertNewItem(item, amount);


    }

    public void OnPlayerDeath()
    {

        Debug.Log("On Player Death");
        movement.SetIsDead(true);
        uIContoller.gameObject.SetActive(false);
        shopUI.gameObject.SetActive(true);
        attack.SetCanAttack(false);

    }
    public void OnReplay()
    {
        GameManager.instance.OnReplay();
        shopUI.gameObject.SetActive(false);
        uIContoller.gameObject.SetActive(true);
        movement.SetIsDead(false);
        uIContoller.gameObject.SetActive(true);
        attack.SetCanAttack(true);
        attack.MaxAmmo();
    }
    private void OnEnable()
    {
        pauseMenuControl.Enable();
    }

    private void OnDisable()
    {
        pauseMenuControl.Disable();
    }
}
