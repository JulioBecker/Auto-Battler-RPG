using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EntityData : ScriptableObject
{
    public int level;
    public int maxHealth;
    public int maxMana;
    public int attack;
    public int magic;
    public int attackSpeed;
    public int critChance;
    public int critDamage;
    public int dodgeChance;
    public int physicResistance;
    public int magicResistance;
    public int speedMultiplier;
    public int rangeMultiplier;
}
