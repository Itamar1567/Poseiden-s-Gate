using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{

    public InputAction action;

    [SerializeField] Item speedUpgrade;

    [SerializeField] float moveSpeed = 5;
    [SerializeField] float moveSpeedUpgradePercentage = 0.15f;
    [SerializeField] float knockBackDelay = 0.3f;

    [SerializeField] Animator wavesAnimation;

    private PlayerController playerController;

    private Rigidbody2D rb;

    private Vector2 moveDirection = Vector2.zero;
    
    private bool isKnockedBack = false;
    private bool isDead = false;

    private float speed;

    private Coroutine knockbackRoutine;

    


    // Start is called before the first frame update
    void Start()
    {
        speed = moveSpeed;
        playerController = GetComponent<PlayerController>();
        wavesAnimation = wavesAnimation.GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        if (isDead)
        {

            moveDirection = Vector2.zero;
            return;

        }

        moveDirection = action.ReadValue<Vector2>();
    }

    //The side the user has chosen the cannon's to fire from
    

    private void FixedUpdate()
    {

            

            if (isKnockedBack) { return; }
        
            speed = moveSpeed + (moveSpeed * moveSpeedUpgradePercentage * playerController.CallGetItemAmount(speedUpgrade));
           
            rb.linearVelocity = new Vector2(moveDirection.x * speed, moveDirection.y * speed);


            bool isMoving = rb.linearVelocity != Vector2.zero;
            wavesAnimation.SetBool("Moving", isMoving);
           

            switch (moveDirection.x)
            {
                case > 0:
                    transform.rotation = Quaternion.Euler(0f, 0f, -45f);
                    break;
                case < 0:
                    transform.rotation = Quaternion.Euler(0f, 0f, 45f);
                    break;
                case 0:
                    transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                    break;
            }

        

    }

    public void KnockedBack(float force, Vector2 direction)
    {

        if (knockbackRoutine != null) { StopCoroutine(knockbackRoutine); }

        rb.linearVelocity = direction * force;
        knockbackRoutine = StartCoroutine(KnockBackDelay());

        
    }

    private IEnumerator KnockBackDelay()
    {
        isKnockedBack = true;
        yield return new WaitForSeconds(knockBackDelay);
        isKnockedBack = false;
    }

    public void SetIsDead(bool dead)
    {
        isDead = dead;
    }

    private void OnEnable()
    {
        action.Enable();
    }

    private void OnDisable()
    {
        action.Disable();
    }
}
