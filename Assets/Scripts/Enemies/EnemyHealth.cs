using UnityEngine;

public class EnemyHealth : Health
{

    [SerializeField] private int itemDropAmount;
    [SerializeField] private Item droppedItem;

    private GameObject lastShotBy;

    // Update is called once per frame
    void Update()
    {
        
    }


    

    public override void Die()
    {
        Inventory inv = lastShotBy.GetComponent<Inventory>();
        if (lastShotBy != null && inv)
        {
            inv.AddToItem(itemDropAmount, droppedItem);
        }

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
