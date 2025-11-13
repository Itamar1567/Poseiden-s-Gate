using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour, Damageable
{


    public GameObject target;
    public Projectile projectilePrefab;
    [SerializeField] Item plank;
    private GameObject lastShotBy;

    //Statistics
    [SerializeField] private int health = 3;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float stoppingDistance = 1.5f;
    [SerializeField] private float immunityTimeAfterHit = 1f;

    [SerializeField] private float waitTimeBetweenFollow = 1f;
    [SerializeField] private float waitTimeBetweenAttack = 1f;

    [SerializeField] private int plankDropAmount = 3;


    //Required gameObjects
    [SerializeField] private Transform parentLeft;
    [SerializeField] private Transform parentRight;

    [SerializeField] private List<Transform> shootPointsLeft = new List<Transform>();
    [SerializeField] private List<Transform> shootPointsRight = new List<Transform>();

    private bool targetDirection = false; // Binary direction: False for left, True for right

    private float lastXPos;

    private bool canFollow = true;
    private bool canAttack = true;
    private bool canBeHit = true;

    public bool hasTakenDamage { get; set; }

    private void Awake()
    {
        foreach(Transform shootPoint in parentLeft)
        {
            shootPointsLeft.Add(shootPoint);
        }
        foreach (Transform shootPoint in parentRight)
        {
            shootPointsRight.Add(shootPoint);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        lastXPos = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
 
        Follow();
        
    }

    private IEnumerator ResumeFollowAfterDelay()
    {
        yield return new WaitForSeconds(waitTimeBetweenFollow);
        canFollow = true;
    }

    private IEnumerator ResumeAttackAfterDelay()
    {
        yield return new WaitForSeconds(waitTimeBetweenAttack);
        canAttack = true;
    }
    private void Attack()
    {
        List<Transform> shootPoints = targetDirection ? shootPointsRight : shootPointsLeft; // False = left, True = right
        foreach(Transform shootPoint in shootPoints)
        {
            Projectile projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
            Vector3 projectSide = targetDirection ? shootPoint.right : -shootPoint.right;
            projectile.Setup(projectSide, gameObject);
        }
    }
    private void Follow()
    {
        float distanceX = Mathf.Abs(target.transform.position.x - transform.position.x);

        //Move torwads the player horizontally
        if (distanceX <= stoppingDistance)
        {

            transform.rotation = Quaternion.Euler(0, 0, 0);

            //The target's y position with respect to self's x position
            Vector3 targetAtY = new Vector3(transform.position.x, target.transform.position.y, 0);

            Vector3 newPos = Vector3.MoveTowards(transform.position , targetAtY, speed * Time.deltaTime);

            transform.position = newPos;

            //Alighn with player's height
            float distanceY = Mathf.Abs(target.transform.position.y - transform.position.y);

            if (distanceY <= 0.5 && canAttack){
                
                canAttack = false;
                Attack();
                StartCoroutine(ResumeAttackAfterDelay());

            }

            Debug.Log("Arrived");
        }
        else
        {
            
            Vector3 newPos = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);

            float moveDirection = newPos.x - lastXPos;

            switch(moveDirection)
            {
                case > 0:

                    transform.rotation = Quaternion.Euler(0,0, -45);
                    targetDirection = true; // Right
                    break;
                case < 0:
                    transform.rotation = Quaternion.Euler(0, 0, 45);
                    targetDirection = false; // Left
                    break;

            }

            transform.position = newPos;

            lastXPos = transform.position.x;
        }
    }

    public void TakeDamage(int damage)
    {
        if(canBeHit)
        {
            canBeHit = false;

            health -= damage;
            if (health <= 0)
            {
                Die();
            }

            StartCoroutine(ImmunityPeriod(immunityTimeAfterHit));
        }
        
        
    }

    public void Die()
    {
        Inventory inv = lastShotBy?.GetComponent<Inventory>();
        if (lastShotBy != null && inv)
        {
            inv.AddToItem(plankDropAmount, plank);
        }
        Destroy(gameObject);
    }

    private IEnumerator ImmunityPeriod(float time)
    {
        yield return new WaitForSeconds(time);
        canBeHit = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (var comp in collision.GetComponents<MonoBehaviour>())
        {
            if (comp is Projectile projectile)
            {
                if(projectile.GetShotBy() != gameObject)
                {
                    lastShotBy = projectile.GetShotBy();
                    Debug.Log(lastShotBy);
                }
            }
        }
    }
}
