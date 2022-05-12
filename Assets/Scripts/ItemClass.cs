using System.Collections;
using UnityEngine;

public abstract class ItemClass : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public bool isStackable;

    public abstract ItemClass GetItem();
    public abstract EquipmentClass GetEquipment();
    public abstract ConsumableClass GetConsumable();
}
