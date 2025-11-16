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

    //Returns true with the change was successfull
    public bool CallChangeItemAmount(Item item, int amount)
    {
        return inventory.ChangeAmountOfItem(item, amount);
    }

    public int CallGetItemAmount(Item item)
    {
        return inventory.GetItemAmount(item);
    }

}
