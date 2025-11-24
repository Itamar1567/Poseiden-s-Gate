using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Purchaseable : MonoBehaviour, IPointerClickHandler
{
    public event Action<Item> OnPurchase;
    
    [SerializeField] private GameObject popupPrefab;

    [SerializeField] private TMP_Text costTxt;
    [SerializeField] private TMP_Text maxStackTxt;
    [SerializeField] private Image itemImage;

    private Item slotItem;

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

    public void SetItemSprt(Sprite sprt)
    {
        itemImage.sprite = sprt;
    }

    public void SetCostTxt(int cost)
    {
        costTxt.text = "Price: " + cost.ToString() + "$";
    }

    public void SetMaxStackTxt(int currentStack, int maxStack)
    {
        if(slotItem.categories == ItemCategories.Projectile) { return; }

        maxStackTxt.text = currentStack.ToString() + "/" + maxStack.ToString();
    }

    public void SetSlotItem(Item item)
    {
        slotItem = item;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            //Check if the user really want to buy the item
            GameObject popup = Instantiate(popupPrefab, canvas.transform);

            if(popup.TryGetComponent(out Popup p)) { p.OnClick += popupConfirmation; }
        }
    }

    private void popupConfirmation(bool confirm)
    {
        //Player accepted
        if (confirm) 
        {
            OnPurchase.Invoke(slotItem);
        }
        
    }

}
