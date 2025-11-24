using Unity.VisualScripting;
using UnityEngine;


public enum ItemCategories
{
    Projectile,
    Material,
    Upgrade,
    Consumable,
}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]

public class Item : ScriptableObject
{

    public ItemCategories categories;
    public string itemName;
    public string description; 
    public Sprite icon;
    public GameObject prefab;

    public float loadTime = 0; // Time to load projectile;
    public int price = 0; //Price if the item is sellable
    public int maxStack = 64;


}
