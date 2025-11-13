using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{

    public string itemName;
    public string description; 
    public Sprite icon;

    public int maxStack = 64;

    public bool isProjectile = false;
    public bool isInUI = false;


}
