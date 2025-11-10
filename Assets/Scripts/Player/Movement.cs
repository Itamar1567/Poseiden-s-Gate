using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{

    public InputAction action;

    [SerializeField] float moveSpeed = 5;

    private Rigidbody2D rb;

    private Vector2 moveDirection = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = action.ReadValue<Vector2>();
    }

    //The side the user has chosen the cannon's to fire from
    

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);

        switch(moveDirection.x)
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

    private void OnEnable()
    {
        action.Enable();
    }

    private void OnDisable()
    {
        action.Disable();
    }
}
