using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{

    [SerializeField] Item itemType;
    [SerializeField] int worth = 1;

    [SerializeField] float downwardSpeed = 5.0f;
    [SerializeField] bool oscillate = true;

    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(downwardSpeed * Time.deltaTime * Vector3.down);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach(var comp in collision.GetComponents<MonoBehaviour>())
        {
            if(comp is Inventory inv)
            {
                inv.ChangeAmountOfItem(itemType, worth);
                Destroy(gameObject);
            }
        }
    }
    private void Oscillate()
    {
        float newHeight = Mathf.Sin(1);
        transform.position = new Vector3(startPos.x, startPos.y + newHeight, startPos.z);
    }
}
