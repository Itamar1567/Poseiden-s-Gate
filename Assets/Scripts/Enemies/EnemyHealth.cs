using System;
using UnityEngine;

public class EnemyHealth : Health
{

    public event Action<string,int,int> OnHealthChanged;
    public event Action<EnemyHealth> OnEnemyDeath;

    [SerializeField] private int itemDropAmount;
    [SerializeField] private Item droppedItem;

    private GameObject lastShotBy;

    // Update is called once per frame
    void Update()
    {
        
    }


    

    public override void Die()
    {
        if (lastShotBy != null && lastShotBy.TryGetComponent(out Inventory inv))
        {
            inv.ChangeAmountOfItem(droppedItem, itemDropAmount);
        }

        OnEnemyDeath?.Invoke(this);
        Destroy(gameObject);
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        string name= "Null";
        if (gameObject.TryGetComponent(out Boss boss))
        {
            name = boss.GetBossName();
        }
        OnHealthChanged?.Invoke(name, currentHealth, maxHealth);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (var comp in collision.GetComponents<MonoBehaviour>())
        {
            if (comp is Projectile projectile)
            {
                if (projectile.GetShotBy() != gameObject)
                {
                    lastShotBy = projectile.GetShotBy();
                    Debug.Log(lastShotBy);
                }
            }
        }
    }

    public int GetCurrentHealth() { 
    
        return currentHealth;

    }
    public int GetMaxHealh()
    {

        return maxHealth;

    }
}
