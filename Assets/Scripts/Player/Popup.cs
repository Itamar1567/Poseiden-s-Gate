using System;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{

    public event Action<bool> OnClick;
   
    public void ClickedNo()
    {
        OnClick.Invoke(false);
        OnClick = null;
        Destroy(gameObject);
    }
    public void ClickedYes()
    {
        OnClick.Invoke(true);
        OnClick = null;
        Destroy(gameObject);
    }




}
