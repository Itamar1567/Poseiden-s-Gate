using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] GameObject playerUIPrefab;
    private UIContoller uIContoller;
    private Movement movement;
    private Attack attack;
    private Health health;
    private Inventory inventory;

    private void Awake()
    {
        GameObject playerUI = Instantiate(playerUIPrefab, transform);
        uIContoller = playerUI.GetComponent<UIContoller>();

        movement = GetComponent<Movement>();
        attack = GetComponent<Attack>();
        health = GetComponent<Health>();
        inventory = GetComponent<Inventory>();

    }
    // Start is called before the first frame update
    void Start()
    {
        if(uIContoller != null)
        {
            uIContoller.BindToPlayer(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CallDisplayShootingSide(float side)
    {
        uIContoller.DisplayShootSide(side);
    }

    //Inventory

    public void CallDisplayItemAmount(int amount, Item item)
    {
        uIContoller.DisplayItemAmount(amount, item);
    }

    public bool CallDecreaseItemAmount(int amount, Item item)
    {
        return inventory.DecreaseItemAmount(amount, item);
    }

    public int CallGetItemAmount(Item item)
    {
        return inventory.GetItemAmount(item);
    }

    public void CallGenerateAmmoDisplay(int amount,Item item)
    {
        uIContoller.GenerateAmmoDisplay(amount, item);
    }

}
