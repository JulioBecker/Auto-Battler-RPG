using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable Class", menuName = "Item/Consumable")]
public class ConsumableClass : ItemClass
{
    public ConsumableClass()
    {
        isStackable = true;
    }
    public override ItemClass GetItem()
    {
        return this;
    }

    public override ConsumableClass GetConsumable()
    {
        return this;
    }

    public override EquipmentClass GetEquipment()
    {
        return null;
    }
}
