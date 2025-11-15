using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerHealth : Health
{
    //Event calls
    public event Action<int, int> OnHealthChanged;
    public event Action<float> OnRepair;

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
        if (Input.GetKeyDown(KeyCode.R))
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
            //Display message : Can't repair shield, because it is already being repaired
        }
        else if (currentShield != maxShield)
        {
            if (playerController.CallDecreaseItemAmount(shieldCost, plank))
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
}

