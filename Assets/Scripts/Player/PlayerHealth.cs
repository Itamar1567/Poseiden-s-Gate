using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Inventory))]
public class PlayerHealth : Health
{

    //Actions
    public InputAction repair;

    //Event calls
    public event Action<int, int> OnHealthChanged;
    public event Action<float> OnRepair;
    public event Action<string> OnPrompt;

    //Items
    [SerializeField] Item plank;

    //Statistics
    [SerializeField] int maxShield = 3;

    [SerializeField] int shieldCost = 3;
    [SerializeField] float repairTime = 1f;


    private PlayerController playerController;
    private int currentShield;

    //Bools
    private bool repairingShield = false;

    private bool hasShield = true;

    // Start is called before the first frame update
    protected override void Start()
    {

        base.Start();

        playerController = GetComponent<PlayerController>();

        currentShield = maxShield;

        OnHealthChanged?.Invoke(currentHealth, currentShield);

    }

    // Update is called once per frame
    void Update()
    {
        if(repair.WasPressedThisFrame())
        {
            RepairShield();
        }
    }

    override public void TakeDamage(int damage)
    {
        if (canBeDamaged == false) { return; }
        
            if (repairingShield)
            {
                StopCoroutine(WaitBeforeShieldRepair());
            }
            if (hasShield)
            {
                hasShield = false;
                currentShield -= damage;
                StartCoroutine(ImmunityPeriod(immunityDelay));
            }
            else
            {
                if (currentShield > 0)
                {
                    hasShield = true;
                }

                base.TakeDamage(damage);

            }

            maxShield = currentHealth;

            OnHealthChanged?.Invoke(currentHealth, currentShield);

    }
    


    

    private void RepairShield()
    {
        if (repairingShield)
        {
            OnPrompt.Invoke("Can't repair shield while it is already being repaired");
        }
        else if (currentShield != maxShield)
        {
            if (playerController.CallChangeItemAmount(plank, -shieldCost))
            {
                repairingShield = true;
                OnRepair.Invoke(repairTime);
                StartCoroutine(WaitBeforeShieldRepair());

            }

        }

    }

    private IEnumerator WaitBeforeShieldRepair()
    {
        yield return new WaitForSeconds(repairTime);
        repairingShield = false;
        currentShield++;
        OnHealthChanged.Invoke(currentShield, currentShield);
        hasShield = true;
    }
    public void GetInitialHealth() { OnHealthChanged.Invoke(currentHealth, currentShield); }

    private void OnEnable()
    {
        repair.Enable();
    }

    private void OnDisable()
    {
        repair.Disable();
    }
}

