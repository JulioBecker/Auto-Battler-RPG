using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntityObject : MonoBehaviour
{
    public List<EntityAttribute> attributes = new List<EntityAttribute>();
    [SerializeField] EntityData entityData;
    //public int level;
    //public int maxHealth;
    //public int maxMana;
    //public int attack;
    //public int magic;
    //public float attackSpeed;
    //public int critChance;
    //public int critDamage;
    //public int dodgeChance;
    //public int physicResistance;
    //public int magicResistance;
    //public int speedMultiplier;
    //public int rangeMultiplier;

    public int currentHealth;
    public int currentMana;
    public float speed;
    public float range;

    public Slider healthBarSlider;

    public void Awake()
    {
        InitAttributes();
        currentHealth = GetAttributeValue(Attribute.maxHealth);
        currentMana = GetAttributeValue(Attribute.maxMana);
        speed = speed * GetAttributeValue(Attribute.speedMultiplier) / 100f;
        range = range * GetAttributeValue(Attribute.rangeMultiplier) / 100f;
        healthBarSlider = GetComponentInChildren<Slider>();
        healthBarSlider.minValue = 0;
        healthBarSlider.maxValue = GetAttributeValue(Attribute.maxHealth);
        healthBarSlider.value = currentHealth;
    }

    private void InitAttributes()
    {
        for(int i = 0; i < EntityAttribute.attributeCount; i++)
        {
            attributes.Add(new EntityAttribute((Attribute)i));
            switch (attributes[i].attribute)
            {
                case Attribute.level:
                    attributes[i].value = entityData.level;
                    break;
                case Attribute.maxHealth:
                    attributes[i].value = entityData.maxHealth;
                    break;
                case Attribute.maxMana:
                    attributes[i].value = entityData.maxMana;
                    break;
                case Attribute.attack:
                    attributes[i].value = entityData.attack;
                    break;
                case Attribute.magic:
                    attributes[i].value = entityData.magic;
                    break;
                case Attribute.attackSpeed:
                    attributes[i].value = entityData.attackSpeed;
                    break;
                case Attribute.critChance:
                    attributes[i].value = entityData.critChance;
                    break;
                case Attribute.critDamage:
                    attributes[i].value = entityData.critDamage;
                    break;
                case Attribute.dodgeChance:
                    attributes[i].value = entityData.dodgeChance;
                    break;
                case Attribute.physicResistance:
                    attributes[i].value = entityData.physicResistance;
                    break;
                case Attribute.magicResistance:
                    attributes[i].value = entityData.magicResistance;
                    break;
                case Attribute.speedMultiplier:
                    attributes[i].value = entityData.speedMultiplier;
                    break;
                case Attribute.rangeMultiplier:
                    attributes[i].value = entityData.rangeMultiplier;
                    break;
                default:
                    break;
            }
        }
    }

    public int GetAttributeValue(Attribute attribute)
    {
        return attributes[(int)attribute].value;
    }

    public void SetAttributeValue(Attribute attribute, int value)
    {
        attributes[(int)attribute].value = value;
    }

    public void calcDamageTaken(int physicDamage, int magicDamage, bool isCrit)
    {
        string textToShow = "";
        Color textColor = Color.white;
        //dodge!
        if (GameUtils.gameUtils.CheckProbability(GetAttributeValue(Attribute.dodgeChance)))
        {
            textToShow = "Dodge!";
        }
        else
        {
            physicDamage = physicDamage > 0 ? Mathf.Max(physicDamage - GetAttributeValue(Attribute.physicResistance), 1) : 0;
            magicDamage = magicDamage > 0 ? Mathf.Max(magicDamage - GetAttributeValue(Attribute.magicResistance), 1) : 0;
            currentHealth -= physicDamage;
            currentHealth -= magicDamage;
            currentHealth = Mathf.Max(currentHealth, 0);
            if (isCrit)
            {
                textToShow = "Crit!\n";
                textColor = Color.red;
            }
            textToShow += (physicDamage + magicDamage).ToString();
        }

        //update healthbar
        healthBarSlider.value = currentHealth;
        //show the damage 
        GameUtils.gameUtils.ShowText(this.gameObject, textToShow, textColor);
    }
}
