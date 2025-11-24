using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Inventory))]
public class PlayerHealth : Health
{

    //Basically a shield for the shield
    [SerializeField] private int shieldUpgrage = 0;

    //Actions
    public InputAction repair;
    public InputAction heal;

    //Event calls
    public event Action<int, int> OnHealthChanged;
    public event Action<float> OnRepair;
    public event Action<string> OnPrompt;
    public event Action OnDie;


    //Items
    [SerializeField] Item plank;
    [SerializeField] Item healPotion;
    [SerializeField] Item healthUpgradeItem;
    [SerializeField] Item shieldUpgradeItem;

    //Statistics
    int maxShield;

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

        shieldUpgrage = playerController.CallGetItemAmount(shieldUpgradeItem);

        maxShield = maxHealth;
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
        if (heal.WasPressedThisFrame())
        {
            Heal();
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
                if (shieldUpgrage > 0) { shieldUpgrage--; }
                else if(shieldUpgrage <= 0) { currentShield -= damage; hasShield = false; }
                StartCoroutine(ImmunityPeriod(immunityDelay));
            }
            else
            {
                if (currentShield > 0)
                {
                    shieldUpgrage = playerController.CallGetItemAmount(shieldUpgradeItem);
                    hasShield = true;
                }

                base.TakeDamage(damage);

            }

            maxShield = currentHealth;

            OnHealthChanged?.Invoke(currentHealth, currentShield);

    }
    public override void Die()
    {
        OnDie.Invoke();
        ResetHealthSystem();
    }
    
    public void Heal()
    {
        if (currentHealth >= maxHealth) { return; }
        if (!playerController.CallChangeItemAmount(healPotion, -1)) { return; }
        
        currentHealth += 1;
        hasShield = false;
        OnHealthChanged.Invoke(currentHealth, currentShield);
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

    public void ResetHealthSystem()
    {
        maxHealth = maxHealth + playerController.CallGetItemAmount(healthUpgradeItem);
        currentHealth = maxHealth;
        maxShield = maxHealth;
        currentShield = maxShield;
        hasShield = true;
        shieldUpgrage = playerController.CallGetItemAmount(shieldUpgradeItem);
        OnHealthChanged?.Invoke(currentHealth, currentShield);

        Debug.Log(currentHealth + " S:" + currentShield);
    }
    public void GetInitialHealth() { OnHealthChanged.Invoke(currentHealth, currentShield); }

    private void OnEnable()
    {
        heal.Enable();
        repair.Enable();
    }

    private void OnDisable()
    {
        heal.Disable();
        repair.Disable();
    }
}

