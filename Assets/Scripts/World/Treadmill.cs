using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Treadmill : MonoBehaviour
{

    public float speed = 2f;        // Speed of water movement
    public float loopPointY = -5f;  // Y position to reset tilemap to
    private Vector3 startPos;        // Original position of tilemap

    void Start()
    {
        startPos = transform.position; // Store starting position
    }

    void Update()
    {
        // Move tilemap down
        transform.position += Vector3.down * speed * Time.deltaTime;

        // If tilemap has moved past the loop point, reset to start
        if (transform.position.y <= loopPointY)
        {
            transform.position = startPos;
        }
    }
}
