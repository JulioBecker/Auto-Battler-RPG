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
    public static int EquipmentTypeCount = 8;

    public int health;
    public int mana;
    public int attack;
    public int magic;
    public float attackSpeed;
    [Range(0f, 1f)] public float critChance;
    public float critDamage;
    [Range(0f, 1f)] public float dodgeChance;
    public int physicResistance;
    public int magicResistance;
    public float speed;
    public float range;

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
