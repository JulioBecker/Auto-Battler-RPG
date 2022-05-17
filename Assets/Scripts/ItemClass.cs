using System.Collections;
using UnityEngine;

public abstract class ItemClass : ScriptableObject
{
    public enum Rarity
    {
        common,
        uncommon,
        rare,
        epic,
        legendary
    }

    public string itemName;
    public Sprite itemIcon;
    public bool isStackable;
    public int itemLevel;
    public Rarity rarity;

    public abstract ItemClass GetItem();
    public abstract EquipmentClass GetEquipment();
    public abstract ConsumableClass GetConsumable();
}
