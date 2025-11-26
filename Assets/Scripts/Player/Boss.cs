using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Boss : MonoBehaviour
{

    [SerializeField] AttackLight attackLightPrefab;

    [SerializeField] string bossName;
    [SerializeField] float timeBetweenAttacks = 9f;
    [SerializeField] int shotsPerAttack = 3;
    [SerializeField] int attackMaxDistanceFromPlayer = 9;

    private List<AttackLight> light2DAttackReferences = new List<AttackLight>();



    private Transform playerTransform;

    private bool canAttack = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if( TryGetComponent(out EnemyHealth health)) { health.OnEnemyDeath += OnDie; }
    }

    // Update is called once per frame
    void Update()
    {
        SetBossPosition();
        Attack();
    }
    private void SetBossPosition()
    {
        if (playerTransform != null) { transform.position = new Vector3(transform.position.x, playerTransform.position.y); }
    }
    //Gives the boss readability for the player's transform
    public void SetPlayerTransformReference(Transform transform)
    {
        playerTransform = transform;
    }

    public string GetBossName()
    {
        return bossName;
    }

    private void Attack()
    {
        if (!canAttack) { return; }

        for (int i = 0; i < shotsPerAttack; i++)
        {
            
            //Gets a random angle from 0 - 360 degrees
            float randAngle = Random.Range(0, Mathf.PI * 2);

            int distance = Random.Range(0, attackMaxDistanceFromPlayer);

            float x = playerTransform.position.x + Mathf.Cos(randAngle) * distance;
            float y = playerTransform.position.y + Mathf.Sin(randAngle) * distance;



            Vector2 spawnPosition = new(x, y);

            AttackLight newLight2DAttack = Instantiate(attackLightPrefab, spawnPosition, Quaternion.identity);

            newLight2DAttack.OnDestroy += RemoveLightReference;

            light2DAttackReferences.Add(newLight2DAttack);


        }

        StartCoroutine(AttackWaitTime());
    }

    private void OnDie(EnemyHealth health)
    {
        foreach(AttackLight attachLight in new List<AttackLight>(light2DAttackReferences))
        {
            RemoveLightReference(attachLight);
        }
    }

    private void RemoveLightReference(AttackLight attackLight)
    {
        attackLight.OnDestroy -= RemoveLightReference;
        light2DAttackReferences.Remove(attackLight);
        Destroy(attackLight.gameObject);
    }
    private IEnumerator AttackWaitTime()
    {
        canAttack = false;
        yield return new WaitForSeconds(timeBetweenAttacks);
        canAttack = true;
    }
}
