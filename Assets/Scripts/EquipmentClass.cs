using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipament Class", menuName = "Item/Equipment")]
public class EquipmentClass : ItemClass
{
    public enum EquipmentType
    {
        Weapon,
        Helmet,
        Chest,
        Legs,
        Boots,
        Gloves,
        Ring,
        Amulet
    }

    public EquipmentType equipType;
    public EquipmentClass()
    {
        isStackable = false;
    }
    public override ItemClass GetItem()
    {
        return this;
    }
    public override EquipmentClass GetEquipment()
    {
        return this;
    }

    public override ConsumableClass GetConsumable()
    {
        return null;
    }
}
