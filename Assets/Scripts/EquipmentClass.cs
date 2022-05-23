using System.Collections;
using System.Collections.Generic;
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

    public List<EntityAttribute> equipAttributes = new List<EntityAttribute>();
    //public int health;
    //public int mana;
    //public int attack;
    //public int magic;
    //public int attackSpeed;
    //public int critChance;
    //public int critDamage;
    //public int dodgeChance;
    //public int physicResistance;
    //public int magicResistance;
    //public int speed;
    //public int range;

    public EquipmentType equipType;
    public EquipmentClass()
    {
        isStackable = false;
        InitAttributes();
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

    private void InitAttributes()
    {
        for (int i = 0; i < EntityAttribute.attributeCount; i++)
        {
            equipAttributes.Add(new EntityAttribute((Attribute)i));
        }
    }
    public int GetAttributeValue(Attribute attribute)
    {
        return equipAttributes[(int)attribute].value;
    }

    public void SetAttributeValue(Attribute attribute, int value)
    {
        equipAttributes[(int)attribute].value = value;
    }
    public void GenerateName()
    {
        switch (equipType)
        {
            case EquipmentClass.EquipmentType.Weapon:
                itemName = "Arma";
                break;
            case EquipmentClass.EquipmentType.Helmet:
                itemName = "Capacete";
                break;
            case EquipmentClass.EquipmentType.Chest:
                itemName = "Peitoral";
                break;
            case EquipmentClass.EquipmentType.Legs:
                itemName = "Calças";
                break;
            case EquipmentClass.EquipmentType.Boots:
                itemName = "Botas";
                break;
            case EquipmentClass.EquipmentType.Gloves:
                itemName = "Luvas";
                break;
            case EquipmentClass.EquipmentType.Ring:
                itemName = "Anel";
                break;
            case EquipmentClass.EquipmentType.Amulet:
                itemName = "Amuleto";
                break;
        }
    }
}
