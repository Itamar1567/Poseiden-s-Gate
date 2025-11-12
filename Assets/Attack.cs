using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Attack : MonoBehaviour
{


    public InputAction side;
    public InputAction attack;

    [SerializeField] Transform rightParent;
    [SerializeField] Transform leftParent;

    [SerializeField] Projectile cannonBallPrefab;
    [SerializeField] int maxAmmo = 9;

    private List<Transform> shootPointsRight = new List<Transform>();
    private List<Transform> shootPointsLeft = new List<Transform>();

    private PlayerController playerController;

    private float shootSide = -1; // -1 for left, 1 for right

    public float attackWaitTime = 1f;
    private bool canAttack = true;
    

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
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
            playerController.CallDisplayShootingSide(shootSide);
        }    
       
        Debug.Log(shootSide);

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

        Debug.Log("Attack");

        List<Transform> shootPointsSide = shootSide < 0 ? shootPointsLeft : shootPointsRight;

        foreach (Transform shootPoint in shootPointsSide)
        {
            Projectile cannonBall = Instantiate(cannonBallPrefab, shootPoint.position, Quaternion.identity);

            Vector3 projectSide = shootSide < 0 ? -shootPoint.right : shootPoint.right;

            cannonBall.Setup(projectSide, gameObject);
          
        }
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
