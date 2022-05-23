using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Attribute
{
     level,
     maxHealth,
     maxMana,
     attack,
     magic,
     attackSpeed,
     critChance,
     critDamage,
     dodgeChance,
     physicResistance,
     magicResistance,
     speedMultiplier,
     rangeMultiplier
}
[System.Serializable]
public class EntityAttribute
{
    public Attribute attribute;
    public string name;
    public int value;
    public static int attributeCount = 13;

    public EntityAttribute(Attribute _attribute)
    {
        attribute = _attribute;
        switch (attribute)
        {
            case Attribute.level:
                name = "N�vel";
                break;
            case Attribute.maxHealth:
                name = "Vida M�xima";
                break;
            case Attribute.maxMana:
                name = "Mana M�xima";
                break;
            case Attribute.attack:
                name = "Ataque";
                break;
            case Attribute.magic:
                name = "Magia";
                break;
            case Attribute.attackSpeed:
                name = "Velocidade de Ataque";
                break;
            case Attribute.critChance:
                name = "Chance de Cr�tico";
                break;
            case Attribute.critDamage:
                name = "Dano Cr�tico";
                break;
            case Attribute.dodgeChance:
                name = "Chance de Esquiva";
                break;
            case Attribute.physicResistance:
                name = "Resist�ncia F�sica";
                break;
            case Attribute.magicResistance:
                name = "Resist�ncia M�gica";
                break;
            case Attribute.speedMultiplier:
                name = "Velocidade";
                break;
            case Attribute.rangeMultiplier:
                name = "Alcance";
                break;
            default:
                throw new System.Exception("Attribute not found");
        }
    }
}
