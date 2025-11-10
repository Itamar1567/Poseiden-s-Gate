using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] GameObject playerUIPrefab;
    private UIContoller uIContoller;
    private Movement movement;
    private Attack attack;

    private void Awake()
    {
        GameObject playerUI = Instantiate(playerUIPrefab, transform);
        uIContoller = playerUI.GetComponent<UIContoller>();

        movement = GetComponent<Movement>();
        attack = GetComponent<Attack>();

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

    public float GetShootSide()
    {
        return attack.GetShootingSide();
    }
}
