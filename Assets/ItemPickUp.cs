using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{

    [SerializeField] bool oscillate = true;

    private Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (oscillate)
        {
            Oscillate();
        }
    }

    private void Oscillate()
    {
        float newHeight = Mathf.Sin(1);
        transform.position = new Vector3(startPos.x, startPos.y + newHeight, startPos.z);
    }
}
