using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUtils : MonoBehaviour
{
    public static GameUtils gameUtils;
    public GameObject floatTextPrefab;
    public PlayerController player;
    public GameObject tooltip;
    [SerializeField] Sprite weaponSprite;
    [SerializeField] Sprite helmetSprite;
    [SerializeField] Sprite chestSprite;
    [SerializeField] Sprite legsSprite;
    [SerializeField] Sprite bootsSprite;
    [SerializeField] Sprite glovesSprite;
    [SerializeField] Sprite ringSprite;
    [SerializeField] Sprite amuletSprite;

    public List<string> basicStats;
    public int basicStatsCount;

    private void Awake()
    {
        if (gameUtils == null) gameUtils = this;
        InitBasicStatsList();
    }
    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }
    public bool CheckProbability(float prob)
    {
        return Random.value <= prob;
    }

    public void ShowText(GameObject go, string text, Color color)
    {
        GameObject floatText = Instantiate(floatTextPrefab, go.transform.position + new Vector3(0, 1.5f), Quaternion.identity);
        floatText.GetComponentInChildren<TextMesh>().color = color;
        floatText.GetComponentInChildren<TextMesh>().text = text;
    }
    private void InitBasicStatsList()
    {
        basicStats.Add("Health");
        basicStats.Add("Mana");
        basicStats.Add("Attack");
        basicStats.Add("Magic");
        basicStats.Add("AttackSpeed");
        basicStats.Add("CritChance");
        basicStats.Add("CritDamage");
        basicStats.Add("Dodge");
        basicStats.Add("PhysicResistance");
        basicStats.Add("MagicResistance");
        basicStats.Add("Speed");
        basicStats.Add("Range");
        basicStatsCount = basicStats.Count;
    }
    public EquipmentClass GenerateEquipment(int level, ItemClass.Rarity rarity)
    {
        EquipmentClass equip = (EquipmentClass)ScriptableObject.CreateInstance("EquipmentClass");
        equip.itemLevel = level;
        equip.rarity = rarity;

        // equipment type (helmet, weapon, chest, etc.)
        equip.equipType = (EquipmentClass.EquipmentType)Random.Range(0, EquipmentClass.EquipmentTypeCount);
        switch (equip.equipType)
        {
            case EquipmentClass.EquipmentType.Weapon:
                equip.itemIcon = weaponSprite;
                equip.itemName = "Arma";
                break;
            case EquipmentClass.EquipmentType.Helmet:
                equip.itemIcon = helmetSprite;
                equip.itemName = "Capacete";
                break;
            case EquipmentClass.EquipmentType.Chest:
                equip.itemIcon = chestSprite;
                equip.itemName = "Peitoral";
                break;
            case EquipmentClass.EquipmentType.Legs:
                equip.itemIcon = legsSprite;
                equip.itemName = "Calças";
                break;
            case EquipmentClass.EquipmentType.Boots:
                equip.itemIcon = bootsSprite;
                equip.itemName = "Botas";
                break;
            case EquipmentClass.EquipmentType.Gloves:
                equip.itemIcon = glovesSprite;
                equip.itemName = "Luvas";
                break;
            case EquipmentClass.EquipmentType.Ring:
                equip.itemIcon = ringSprite;
                equip.itemName = "Anel";
                break;
            case EquipmentClass.EquipmentType.Amulet:
                equip.itemIcon = amuletSprite;
                equip.itemName = "Amuleto";
                break;
        }

        // Attribute draw based in the equip rarity
        int qtyAttributes = (int)rarity + 1;
        List<string> itemAttributes = new List<string>();
        for(int i = 0; i < qtyAttributes; i++)
        {
            string attribute = basicStats[Random.Range(0, basicStatsCount)];
            while (itemAttributes.Contains(attribute))
            {
                attribute = basicStats[Random.Range(0, basicStatsCount)];
            }
            itemAttributes.Add(attribute);

            switch (attribute)
            {
                case "Health":
                    equip.health = (int)GetStatusValue(false);
                    break;
                case "Mana":
                    equip.mana = (int)GetStatusValue(false);
                    break;
                case "Attack":
                    equip.attack = (int)GetStatusValue(false);
                    break;
                case "Magic":
                    equip.magic = (int)GetStatusValue(false);
                    break;
                case "AttackSpeed":
                    equip.attackSpeed = (float)GetStatusValue(true);
                    break;
                case "CritChance":
                    equip.critChance = (float)GetStatusValue(true);
                    break;
                case "CritDamage":
                    equip.critDamage = (float)GetStatusValue(true);
                    break;
                case "Dodge":
                    equip.dodgeChance = (float)GetStatusValue(true);
                    break;
                case "PhysicResistance":
                    equip.physicResistance = (int)GetStatusValue(false);
                    break;
                case "MagicResistance":
                    equip.magicResistance = (int)GetStatusValue(false);
                    break;
                case "Speed":
                    equip.speed = (float)GetStatusValue(true);
                    break;
                case "Range":
                    equip.range = (float)GetStatusValue(true);
                    break;
            }
        }

        return equip;
    }

    private object GetStatusValue(bool isFloat)
    {
        int value = Random.Range(1, 11);
        if (isFloat)
        {
            return value / 100f;
        }
        else
        {
            return value;
        }
    }

    public void ShowTooltip(string text, Vector3 position)
    {
        tooltip.transform.position = position;
        tooltip.GetComponentInChildren<Text>().text = text;
        tooltip.SetActive(true);
    }

    public void HideTooltip()
    {
        tooltip.SetActive(false);
    }
}
