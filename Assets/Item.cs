using UnityEngine;


public enum ItemCategories
{
    Projectile,
    Material,
}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]

public class Item : ScriptableObject
{

    public ItemCategories categories;
    public string itemName;
    public string description; 
    public Sprite icon;

    public int maxStack = 64;



}
