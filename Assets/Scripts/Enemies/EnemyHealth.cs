using System;
using UnityEngine;

public class EnemyHealth : Health
{

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

        OnEnemyDeath.Invoke(this);
        Destroy(gameObject);
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
}
