using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public event Action OnPlay;

    public event Action<Item, int> OnBoughtNewItem;
    public event Action<Item> OnBoughtUpgrade;

    [SerializeField] float promptDecayTime = 3f;

    [SerializeField] private Item coin;

    [SerializeField] private GameObject shop;
    [SerializeField] private GameObject menu;

    [SerializeField] private GameObject ImageOverlay;

    [SerializeField] private List<Item> sellableItems;
    [SerializeField] private Transform shopGrid;
    [SerializeField] private Purchaseable purchaseableSlotPrefab;

    [SerializeField] private TMP_Text coinsAmountTxt;
    [SerializeField] private TMP_Text promptTxt;

    private List<Purchaseable> purchaseablesRef = new List<Purchaseable>();

    private Inventory inventory;

    private Coroutine textFadeOut;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitiateShop();
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPrompt(string text)
    {
        promptTxt.text = text;
        if (textFadeOut != null) { StopCoroutine(textFadeOut); }
        FadeOutText(promptDecayTime, promptTxt);
    }
    public void FadeOutText(float timeToFade, TMP_Text text)
    {
        textFadeOut = StartCoroutine(FadeOutNumeratorText(timeToFade, text));
    }

    private IEnumerator FadeOutNumeratorText(float time, TMP_Text text)
    {

        text.color = Color.white;

        Color initialColor = text.color;
        float elapsed = 0f;

        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(initialColor.a, 0f, elapsed / time);
            text.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
            yield return null;
        }

        // Ensure fully transparent at the end
        text.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);
    }
    public void ReplayButton()
    {
        OnPlay.Invoke();
    }
    public void OpenShopButton()
    {
        coinsAmountTxt.text = inventory.GetItemAmount(coin).ToString();
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
            newSlot.SetPlayerInventoryRef(inventory);
            int currentStack = inventory.GetItemAmount(item);
            newSlot.SetMaxStackTxt();
            newSlot.OnPrompt += SetPrompt;
            newSlot.OnPurchase += Transact;
            newSlot.OnPromptGeneration += EnableOrDisablePurchases;
            Debug.Log("entered");
            purchaseablesRef.Add(newSlot);

        }
    }

    private void EnableOrDisablePurchases(bool onOrOff)
    {
        ImageOverlay.SetActive(onOrOff);
    }

    public void SetPlayerInventoryReference(Inventory inv)
    {
        inventory = inv;
    }
    private void Transact(Item item)
    {
        coinsAmountTxt.text = inventory.GetItemAmount(coin).ToString();

        switch (item.categories)
        {
            case ItemCategories.Upgrade:
                OnBoughtNewItem.Invoke(item, 1);
                break;
            case ItemCategories.Consumable:
                OnBoughtNewItem.Invoke(item, 1);
                break;
            default:
                OnBoughtNewItem.Invoke(item, item.maxStack);
                break;
        }
    }
}
