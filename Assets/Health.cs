using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, Damageable
{
    [SerializeField] int maxHealth = 3;
    [SerializeField] int shield = 0;

    [SerializeField] float immunityTimeAfterHit = 1f;

    private PlayerController playerController;

    private int health;
    private bool canBeDamaged = true;

    public bool hasTakenDamage { get; set; }

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

            if (shield != 0)
            {
                shield -= damage;
            }
            else
            {
                health -= damage;
                playerController.CallDestroyHearts(damage);
            }

            if (health <= 0)
            {
                Die();
            }

            StartCoroutine(ImmunityPeriod(immunityTimeAfterHit));
        }
    }


    private IEnumerator ImmunityPeriod(float time)
    {
        yield return new WaitForSeconds(time);
        canBeDamaged = true;
    }
    public int GetHealth() { return health; }
    public int GetShield() { return shield; }

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        health = maxHealth;
        playerController.CallGenerateHearts(health);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
