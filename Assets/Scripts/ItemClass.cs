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
    public Rarity rarity;

    public abstract ItemClass GetItem();
    public abstract EquipmentClass GetEquipment();
    public abstract ConsumableClass GetConsumable();

    public string GetColor()
    {
        string color = string.Empty;
        switch (rarity)
        {
            case ItemClass.Rarity.common:
                color = "#666666";
                break;
            case ItemClass.Rarity.uncommon:
                color = "#00ff00";
                break;
            case ItemClass.Rarity.rare:
                color = "#0000ff";
                break;
            case ItemClass.Rarity.epic:
                color = "#880080";
                break;
            case ItemClass.Rarity.legendary:
                color = "#f06316";
                break;
        }
        return color;
    }
}
