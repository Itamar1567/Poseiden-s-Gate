using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour, Damageable
{

    //Statistics
    [SerializeField] protected int maxHealth = 3;
    [SerializeField] protected float immunityDelay = 0.5f;

    private SpriteRenderer spriteRenderer;

    protected int currentHealth;
    protected bool canBeDamaged = true;
    public bool hasTakenDamage { get; set; }

    
    // Start is called before the first frame update
    protected virtual void Awake()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();

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
        Color color = spriteRenderer.color;

        //Display Immunity period

        float timePassed = 0f;
        float minAlpha = 0.5f;
        float maxAlpha = 1f;

        while (timePassed < time)
        {
            float newAlpha = color.a < maxAlpha ? maxAlpha : minAlpha;

            while (!Mathf.Approximately(color.a, newAlpha))
            {
                color.a = Mathf.MoveTowards(color.a, newAlpha, Time.deltaTime);
                spriteRenderer.color = color;
            }

            timePassed += Time.deltaTime;
            yield return null;

        }

        color.a = maxAlpha;

        spriteRenderer.color = color;

        canBeDamaged = true;

        yield return null;
        

        
        
    }

}
