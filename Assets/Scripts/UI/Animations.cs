using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animations : MonoBehaviour
{

    [SerializeField] float amplitude = 1f;
    [SerializeField] float speed = 1f;
    public bool oscillateOnY = false;

    private Vector3 initialPosition;
    // Start is called before the first frame update
    void Start()
    {
        
        initialPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(oscillateOnY){ OscillateOnY(); }
    }

    public void OscillateOnY()
    {
        gameObject.transform.localPosition = new Vector3(initialPosition.x, initialPosition.y + Mathf.Sin(Time.time * speed) * amplitude, 0);
    }

    public void SetInitialPosition(Vector3 pos)
    {
        initialPosition = pos;
    }
}
