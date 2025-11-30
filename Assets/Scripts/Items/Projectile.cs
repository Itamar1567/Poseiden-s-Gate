using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{


    [SerializeField] int damage = 15;
    [SerializeField] float speed = 5f;

    private Rigidbody2D rb;
    private GameObject shotBy;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); 
    }

    private void Start()
    {
        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Setup(Vector3 direction, GameObject s)
    {
        Debug.Log("Shot");
        rb.linearVelocity = direction * speed;
        shotBy = s;
    }

    private void DamageTarget(Collider2D collided)
    {
        foreach(MonoBehaviour comp in collided.GetComponents<MonoBehaviour>())
        {
            if(comp is Damageable damageable)
            {
                damageable.TakeDamage(damage);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject != shotBy)
        {
            if (collision.TryGetComponent(out AttackLight al)) { return; }
            //Debug.Log(shotBy, collision.gameObject);
            DamageTarget(collision);
            Destroy(gameObject);
        }   
    }

    public GameObject GetShotBy()
    {
        if(shotBy != null)
        {
            return shotBy;
        }
        else
        {
            Debug.Log("shot by is null");
            return null;
        }
    }

}
