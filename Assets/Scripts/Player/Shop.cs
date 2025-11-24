using System;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public event Action OnPlay;

    public event Action<Item, int> OnBoughtNewItem;
    public event Action<Item> OnBoughtUpgrade;

    [SerializeField] private GameObject shop;
    [SerializeField] private GameObject menu;

    [SerializeField] private List<Item> sellableItems;
    [SerializeField] private Transform shopGrid;
    [SerializeField] private Purchaseable purchaseableSlotPrefab;

    private Inventory inventory;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitiateShop();
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReplayButton()
    {
        OnPlay.Invoke();
    }
    public void OpenShopButton()
    {
        shop.SetActive(true);
        menu.SetActive(false);
    }
    public void CloseShopButton()
    {
        shop.SetActive(false);
        menu.SetActive(true);
    }
    public void QuitButton()
    {
        Application.Quit();
    }

    private void InitiateShop()
    {
        foreach(Item item in sellableItems)
        {
            Purchaseable newSlot = Instantiate(purchaseableSlotPrefab, shopGrid);
            newSlot.SetItemSprt(item.icon);
            newSlot.SetCostTxt(item.price);
            newSlot.SetSlotItem(item);
            int currentStack = inventory.GetItemAmount(item);
            newSlot.SetMaxStackTxt(currentStack, item.maxStack);
            newSlot.OnPurchase += Transact;
            
        }
    }

    public void SetPlayerInventoryReference(Inventory inv)
    {
        inventory = inv;
    }
    private void Transact(Item item)
    {
        switch (item.categories)
        {
            case ItemCategories.Material:
                OnBoughtNewItem.Invoke(item, item.maxStack);
                    break;
            case ItemCategories.Upgrade:
                OnBoughtUpgrade.Invoke(item);
                break;
        }
    }
}
