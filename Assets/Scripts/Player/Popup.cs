using System;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{

    [SerializeField] private AudioClip clickSound;

    private AudioSource audioSource;

    public event Action<bool> OnClick;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();    
    }

    public void ClickedNo()
    {
        audioSource.PlayOneShot(clickSound);
        OnClick.Invoke(false);
        OnClick = null;
        Destroy(gameObject);
    }
    public void ClickedYes()
    {
        audioSource.PlayOneShot(clickSound);
        OnClick.Invoke(true);
        OnClick = null;
        Destroy(gameObject);
    }




}
