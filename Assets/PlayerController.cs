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


    //Health
    public void CallDisplayTakeDamage(int amount)
    {
        uIContoller.DisplayTakeDamage(amount);
    }

    public void CallGenerateHearts(int health)
    {
        uIContoller.GenerateHearts(health);
    }

    public void CallGenerateShields(int shield)
    {
        uIContoller.GenerateShields(shield);
    }

    //Inventory

    public void CallDisplayItemAmount(int amount, string type)
    {
        uIContoller.DisplayItemAmount(amount, type);
    }

    public bool CallDecreaseItemAmount(int amount, string type)
    {
        return inventory.DecreaseItemAmount(amount, type);
    }

    public void CallGetItemAmount(string type)
    {
        inventory.GetItemAmount(type);
    }
}
