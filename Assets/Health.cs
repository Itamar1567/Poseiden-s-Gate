using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, Damageable
{
    [SerializeField] int maxHealth = 3;
    [SerializeField] int maxShield = 3;

    [SerializeField] float immunityTimeAfterHit = 1f;

    [SerializeField] int shieldCost = 3;
    [SerializeField] float repairTime = 1f;

    private PlayerController playerController;

    private int health;
    private int shield;
    private bool canBeDamaged = true;
    private bool repairingShield = false;

    private bool hasShield = true;
    public bool hasTakenDamage { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();

        health = maxHealth;
        shield = maxShield;

        playerController.CallGenerateHearts(health);
        playerController.CallGenerateShields(shield);
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

            Debug.Log("Hit");

            playerController.CallDisplayTakeDamage(damage);

            if(repairingShield)
            {
                StopCoroutine(WaitBeforeShieldRepair());
            }
            if (hasShield)
            {
                hasShield = false;
                shield -= damage;
            }
            else
            {
                if(shield > 0)
                {
                    hasShield = true;
                }
                health -= damage;
            }

            if (health <= 0)
            {
                Die();
            }

            maxShield = health;
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
        else if (shield != maxShield)
        {
            if(playerController.CallDecreaseItemAmount(shieldCost, "log"))
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
        playerController.CallGenerateShields(1);
        shield++;
        hasShield = true;
    }
    public int GetHealth() { return health; }
    public int GetShield() { return shield; }

    
}
