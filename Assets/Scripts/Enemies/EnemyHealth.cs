using System;
using System.Collections;
using UnityEngine;

public class EnemyHealth : Health
{

    [SerializeField] AnimationClip dieAnimationClip;

    public event Action<string,int,int> OnHealthChanged;
    public event Action<EnemyHealth> OnEnemyDeath;

    [SerializeField] private int itemDropAmount;
    [SerializeField] private Item droppedItem;

    private GameObject lastShotBy;

    private bool isDead = false;

    // Update is called once per frame
    void Update()
    {
        
    }


    private IEnumerator DieAnimation()
    {
        animator.SetTrigger("OnDie");

        yield return new WaitForSeconds(dieAnimationClip.length);

        Destroy(gameObject);
    }

    public override void Die()
    {

        isDead = true;

        if (lastShotBy != null && lastShotBy.TryGetComponent(out Inventory inv))
        {
            inv.ChangeAmountOfItem(droppedItem, itemDropAmount);
        }

        OnEnemyDeath?.Invoke(this);

        if (animator != null)
        {
            StartCoroutine(DieAnimation());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public override void TakeDamage(int damage)
    {
        if (!isDead)
        {
            base.TakeDamage(damage);
            string name = "Null";
            if (gameObject.TryGetComponent(out Boss boss))
            {
                name = boss.GetBossName();
            }
            OnHealthChanged?.Invoke(name, currentHealth, maxHealth);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isDead) 
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
    }

    public int GetCurrentHealth() { 
    
        return currentHealth;

    }
    public int GetMaxHealh()
    {

        return maxHealth;

    }
    public bool GetIsEnemyDead() {
        return isDead;
    }
}
