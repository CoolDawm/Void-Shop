using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class SO_Item : ScriptableObject
{
    public string itemName;
    public Sprite itemImage;
    public int weight;
    public string description;
}