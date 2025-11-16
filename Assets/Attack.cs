using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[RequireComponent(typeof(Inventory))]
public class Attack : MonoBehaviour
{

    PlayerController controller;

    public event Action<float> OnChangeSide;
    public event Action<float> OnShoot;

    public InputAction side;
    public InputAction attack;

    [SerializeField] Transform rightParent;
    [SerializeField] Transform leftParent;

    [SerializeField] Projectile cannonBallPrefab;
    [SerializeField] Item cannonBall;

    readonly private List<Transform> shootPointsRight = new List<Transform>();
    readonly private List<Transform> shootPointsLeft = new List<Transform>();

    private float shootSide = -1; // -1 for left, 1 for right

    public float attackWaitTime = 1f;
    private bool canAttack = true;
    

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<PlayerController>();
    }


    private void Awake()
    {
        foreach(Transform child in rightParent)
        {
            shootPointsRight.Add(child);
        }
        foreach (Transform child in leftParent)
        {
            shootPointsLeft.Add(child);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(side.WasPressedThisFrame())
        {
            shootSide = side.ReadValue<float>();
            OnChangeSide.Invoke(shootSide);
        }    
       
        if (attack.WasPressedThisFrame() && canAttack)
        {
            canAttack = false;
            Shoot();
            StartCoroutine(WaitToShoot());
        }

    }

    public float GetShootingSide()
    {
        return shootSide;
    }

    private IEnumerator WaitToShoot()
    {
        yield return new WaitForSeconds(attackWaitTime);
        canAttack = true;
    }

    void Shoot()
    {

        List<Transform> shootPointsSide = shootSide < 0 ? shootPointsLeft : shootPointsRight;

        foreach (Transform shootPoint in shootPointsSide)
        {
            Projectile projectile = Instantiate(cannonBallPrefab, shootPoint.position, Quaternion.identity);

            Vector3 projectSide = shootSide < 0 ? -shootPoint.right : shootPoint.right;

            projectile.Setup(projectSide, gameObject);
          
        }

        controller.CallChangeItemAmount(cannonBall, -1);
        OnShoot.Invoke(attackWaitTime);

    }

    private void OnEnable()
    {
        side.Enable();
        attack.Enable();
    }
    private void OnDisable()
    {
        side.Disable();
        attack.Disable();
    }
}
