using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, Damageable
{

    //Statistics
    [SerializeField] protected int maxHealth = 3;
    [SerializeField] protected float immunityDelay = 0.5f;


    protected int currentHealth;
    protected bool canBeDamaged = true;
    public bool hasTakenDamage { get; set; }

    
    // Start is called before the first frame update
    protected virtual void Awake()
    {

        currentHealth = maxHealth;

    }

    // Update is called once per frame
    void Update()
    {

    }

    virtual public void Die()
    {
        Destroy(gameObject);
    }

    virtual public void TakeDamage(int damage)
    {

        if (canBeDamaged == false) { return; }
        
            currentHealth -= damage;

            
            if (currentHealth <= 0)
            {
                Die();
            }

            StartCoroutine(ImmunityPeriod(immunityDelay));
        
    }

    public IEnumerator ImmunityPeriod(float time)
    {
        canBeDamaged = false;
        yield return new WaitForSeconds(time);
        canBeDamaged = true;
    }

}
