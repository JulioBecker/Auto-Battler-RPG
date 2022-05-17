using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntityObject : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    public int maxMana;
    public int currentMana;
    public int attack;
    public int magic;
    public float attackSpeed;
    [Range(0f, 1f)] public float critChance;
    [Min(1.1f)] public float critDamage;
    [Range(0f, 1f)] public float dodgeChance;
    public int physicResistance;
    public int magicResistance;
    public float speed;
    public float range;
    public Slider healthBarSlider;
    GameUtils gameUtils;

    public void Awake()
    {
        gameUtils = FindObjectOfType<GameUtils>();
        currentHealth = maxHealth;
        currentMana = maxMana;
        healthBarSlider = GetComponentInChildren<Slider>();
        healthBarSlider.minValue = 0;
        healthBarSlider.maxValue = maxHealth;
        healthBarSlider.value = currentHealth;
    }

    public void calcDamageTaken(int physicDamage, int magicDamage, bool isCrit)
    {
        string textToShow = "";
        Color textColor = Color.white;
        //dodge!
        if (gameUtils.CheckProbability(dodgeChance))
        {
            textToShow = "Dodge!";
        }
        else
        {
            physicDamage = physicDamage > 0 ? Mathf.Max(physicDamage - physicResistance, 1) : 0;
            magicDamage = magicDamage > 0 ? Mathf.Max(magicDamage - magicResistance, 1) : 0;
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
        gameUtils.ShowText(this.gameObject, textToShow, textColor);
    }
}
