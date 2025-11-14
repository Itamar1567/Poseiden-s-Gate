using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, Damageable
{

    //Event calls
    public event Action<int,int> OnHealthChanged;

    //Items
    [SerializeField] Item plank;

    //Statistics
    [SerializeField] int maxHealth = 3;
    [SerializeField] int maxShield = 3;

    [SerializeField] float immunityTimeAfterHit = 1f;

    [SerializeField] int shieldCost = 3;
    [SerializeField] float repairTime = 1f;


    private PlayerController playerController;

    private int currentHealth;
    private int currentShield;

    //Bools

    private bool canBeDamaged = true;
    private bool repairingShield = false;

    private bool hasShield = true;
    public bool hasTakenDamage { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();

        currentHealth = maxHealth;
        currentShield = maxShield;

        OnHealthChanged.Invoke(currentHealth, currentShield);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RepairShield();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        if (canBeDamaged)
        {
            canBeDamaged = false;

            if(repairingShield)
            {
                StopCoroutine(WaitBeforeShieldRepair());
            }
            if (hasShield)
            {
                hasShield = false;
                currentShield -= damage;
                
            }
            else
            {
                if(currentShield > 0)
                {
                    hasShield = true;
                }
                currentHealth -= damage;
                
            }

            if (currentHealth <= 0)
            {
                Die();
            }

            maxShield = currentHealth;

            OnHealthChanged.Invoke(currentHealth, currentShield);

            StartCoroutine(ImmunityPeriod(immunityTimeAfterHit));
        }
    }


    private IEnumerator ImmunityPeriod(float time)
    {
        yield return new WaitForSeconds(time);
        canBeDamaged = true;
    }

    private void RepairShield()
    {
        if (repairingShield)
        {
            //Display message
        }
        else if (currentShield != maxShield)
        {
            if(playerController.CallDecreaseItemAmount(shieldCost, plank))
            {
                repairingShield = true;
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
