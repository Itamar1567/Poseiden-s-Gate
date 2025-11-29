using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{

    [SerializeField] private AudioClip soundClip;

    [SerializeField] Item itemType;
    [SerializeField] int worth = 1;

    [SerializeField] float downwardSpeed = 5.0f;
    [SerializeField] bool oscillate = true;

    AudioSource audioSource;

    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(downwardSpeed * Time.deltaTime * Vector3.down);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out PlayerController playerController)){
            if (itemType.categories == ItemCategories.Projectile) { playerController.MaxAmmo(); }
            playerController.CallChangeItemAmount(itemType, worth);
            audioSource.PlayOneShot(soundClip);
            GetComponent<SpriteRenderer>().enabled = false;
            Destroy(gameObject,3f);
        }
    }
}
