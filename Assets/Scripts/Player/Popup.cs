using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{

    [SerializeField] private AudioClip clickSound;

    [SerializeField] TMP_Text itemName;
    [SerializeField] TMP_Text itemDescription;

    private AudioSource audioSource;

    public event Action<bool> OnClick;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();    
    }

    public void SetItemNameAndDescription(string name, string desc)
    {
        itemName.text = name + ":";
        itemDescription.text = desc;
    }

    public void ClickedNo()
    {
        audioSource.PlayOneShot(clickSound);
        OnClick.Invoke(false);
        Destroy(gameObject);
    }
    public void ClickedYes()
    {
        audioSource.PlayOneShot(clickSound);
        OnClick.Invoke(true);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        OnClick = null;
    }




}
