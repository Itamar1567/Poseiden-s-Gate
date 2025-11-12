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

    private void Awake()
    {
        GameObject playerUI = Instantiate(playerUIPrefab, transform);
        uIContoller = playerUI.GetComponent<UIContoller>();

        movement = GetComponent<Movement>();
        attack = GetComponent<Attack>();
        health = GetComponent<Health>();

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

    public void CallDestroyHearts(int amount)
    {
        uIContoller.DestroyHearts(amount);
    }

    public void CallGenerateHearts(int health)
    {
        uIContoller.GenerateHearts(health);
    }
}
