using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerController))]
public class Attack : MonoBehaviour
{

    PlayerController controller;

    public event Action<float> OnChangeSide;
    public event Action<float> OnShoot;
    public event Action<Item, int> OnChangeProjectile;


    public InputAction side;
    public InputAction changeWeapon;
    public InputAction attack;

    [SerializeField] private AudioClip shotSound;

    [SerializeField] Transform rightParent;
    [SerializeField] Transform leftParent;

    [SerializeField] private List<Item> projectiles;
    private int chosenProjectileIndex = 0;

    readonly private List<Transform> shootPointsRight = new List<Transform>();
    readonly private List<Transform> shootPointsLeft = new List<Transform>();

    private float shootSide = -1; // -1 for left, 1 for right

    private bool canAttack = true;

    private Animator animator;

    private AudioSource audioSource;
    

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        controller = GetComponent<PlayerController>();
        MaxAmmo();
        
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
        if (!canAttack) { return; }

        if(side.WasPressedThisFrame())
        {
            shootSide = side.ReadValue<float>();
            OnChangeSide.Invoke(shootSide);
        }
        if (changeWeapon.WasPressedThisFrame())
        {
            //either -1 or 1
            float moveTo = changeWeapon.ReadValue<float>();

            ChangeProjectile(moveTo);
        }
       
        if (attack.WasPressedThisFrame() && canAttack){ Shoot(); }

    }

    public float GetShootingSide()
    {
        return shootSide;
    }

    public Item GetCurrentProjectile()
    {
        return projectiles[chosenProjectileIndex];
    }

    private IEnumerator WaitToShoot(float waitTime)
    {
        canAttack = false;
        yield return new WaitForSeconds(waitTime);
        canAttack = true;
    }

    void Shoot()
    {

        Item chosenProjectile = projectiles[chosenProjectileIndex];

        if (controller.CallChangeItemAmount(chosenProjectile, -1))
        {
            string shootTrigger = shootSide < 0 ? "Shoot_Left" : "Shoot_Right";
            animator.SetTrigger(shootTrigger);

            audioSource.PlayOneShot(shotSound);

            List<Transform> shootPointsSide = shootSide < 0 ? shootPointsLeft : shootPointsRight;

            foreach (Transform shootPoint in shootPointsSide)
            {
                //If the item is a projectile / has a projectile script attached enter this
                if (chosenProjectile.prefab.TryGetComponent(out Projectile prefab)){

                    Projectile projectile = Instantiate(prefab, shootPoint.position, Quaternion.identity);

                    Vector3 projectSide = shootSide < 0 ? -shootPoint.right : shootPoint.right;

                    projectile.Setup(projectSide, gameObject);

                }
            }

            StartCoroutine(WaitToShoot(chosenProjectile.loadTime));
            OnShoot.Invoke(chosenProjectile.loadTime);
        }

    }

    private void MaxAmmo()
    {
        foreach(Item item in projectiles)
        {
            controller.CallChangeItemAmount(item, item.maxStack);
        }
    }
    private void ChangeProjectile(float moveTo)
    {
        chosenProjectileIndex += (int)moveTo;
        if (chosenProjectileIndex < 0) { chosenProjectileIndex = projectiles.Count - 1; }
        if (chosenProjectileIndex > projectiles.Count - 1) { chosenProjectileIndex = 0; }

        Item item = projectiles[chosenProjectileIndex];
        OnChangeProjectile.Invoke(item, controller.CallGetItemAmount(item));
        Debug.Log(chosenProjectileIndex);
    }

    public void AddNewProjectile(Item item)
    {
        Debug.Log("Added projectile");
        projectiles.Add(item);
    }

    public void SetCanAttack(bool ca)
    {
        canAttack = ca;
    }
    private void OnEnable()
    {
        changeWeapon.Enable();
        side.Enable();
        attack.Enable();
    }
    private void OnDisable()
    {
        changeWeapon.Disable();
        side.Disable();
        attack.Disable();
    }
}
