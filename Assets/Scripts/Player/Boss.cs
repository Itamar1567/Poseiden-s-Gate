using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{

    [SerializeField] string bossName;
    [SerializeField] float timeBetweenAttacks = 5f;


    private Transform playerTransform;

    private bool canAttack;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetBossPosition();
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
    private IEnumerator AttackWaitTime()
    {
        canAttack = false;
        yield return new WaitForSeconds(timeBetweenAttacks);
        canAttack = true;
    }
}
