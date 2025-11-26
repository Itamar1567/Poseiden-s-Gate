using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Purchaseable : MonoBehaviour, IPointerClickHandler
{

    public event Action<string> OnPrompt;

    public event Action<bool> OnPromptGeneration;

    public event Action<Item> OnPurchase;
    
    [SerializeField] private GameObject popupPrefab;

    [SerializeField] private TMP_Text costTxt;
    [SerializeField] private TMP_Text maxStackTxt;
    [SerializeField] private Image itemImage;

    [SerializeField] private Item coin;

    private Item slotItem;

    private Inventory inventory;

    private Canvas canvas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvas = FindFirstObjectByType<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayerInventoryRef(Inventory inv)
    {
        inventory = inv;
    }

    public void SetItemSprt(Sprite sprt)
    {
        itemImage.sprite = sprt;
    }

    public void SetCostTxt(int cost)
    {
        costTxt.text = "Price: " + cost.ToString() + "$";
    }

    public void SetMaxStackTxt()
    {
        if(slotItem.categories == ItemCategories.Projectile) { return; }

        maxStackTxt.text = inventory.GetItemAmount(slotItem).ToString() + "/" + slotItem.maxStack.ToString();
    }

    public void SetSlotItem(Item item)
    {
        slotItem = item;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            if (inventory.GetItemAmount(coin) < slotItem.price) { OnPrompt.Invoke("You don't have enough coins for this item"); return; }

            OnPromptGeneration.Invoke(true);

            //Check if the user really want to buy the item
            GameObject popup = Instantiate(popupPrefab, canvas.transform);

            if(popup.TryGetComponent(out Popup p)) { p.OnClick += PopupConfirmation; }
        }
    }

    private void PopupConfirmation(bool confirm)
    {
        //Player accepted
        if (confirm) 
        {
            Debug.Log(slotItem);
            inventory.ChangeAmountOfItem(coin, -slotItem.price);
            OnPurchase.Invoke(slotItem);
            SetMaxStackTxt();
        }

        OnPromptGeneration.Invoke(false);

    }

}
