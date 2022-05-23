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
    }
    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }
    public bool CheckProbability(int prob)
    {
        return Random.Range(1, 101) <= prob;
    }

    public int IndexProbabilityTable(float[] table)
    {
        int index = table.Length - 1;
        float roll = Random.value;
        for(int i = 0; i < table.Length - 1; i++)
        {
            if (roll <= table[i]) return index;
            index--;
        }
        return index;
    }

    public void ShowText(GameObject go, string text, Color color)
    {
        GameObject floatText = Instantiate(floatTextPrefab, go.transform.position + new Vector3(0, 1.5f), Quaternion.identity);
        floatText.GetComponentInChildren<TextMesh>().color = color;
        floatText.GetComponentInChildren<TextMesh>().text = text;
    }

    public EquipmentClass GenerateEquipment(int level, ItemClass.Rarity rarity)
    {
        EquipmentClass equip = (EquipmentClass)ScriptableObject.CreateInstance("EquipmentClass");
        equip.SetAttributeValue(Attribute.level, level);
        equip.rarity = rarity;

        //List<EntityAttribute> itemAttributes = new List<EntityAttribute>();

        // equipment type (helmet, weapon, chest, etc.)
        equip.equipType = (EquipmentClass.EquipmentType)Random.Range(0, EquipmentClass.EquipmentTypeCount);
        switch (equip.equipType)
        {
            case EquipmentClass.EquipmentType.Weapon:
                equip.itemIcon = weaponSprite;
                //itemAttributes.Add("Attack");
                equip.SetAttributeValue(Attribute.attack, GetStatusValue());
                break;
            case EquipmentClass.EquipmentType.Helmet:
                equip.itemIcon = helmetSprite;
                //itemAttributes.Add("PhysicResistance");
                equip.SetAttributeValue(Attribute.physicResistance, GetStatusValue());
                break;
            case EquipmentClass.EquipmentType.Chest:
                equip.itemIcon = chestSprite;
                //itemAttributes.Add("PhysicResistance");
                equip.SetAttributeValue(Attribute.physicResistance, GetStatusValue());
                break;
            case EquipmentClass.EquipmentType.Legs:
                equip.itemIcon = legsSprite;
                //itemAttributes.Add("PhysicResistance");
                equip.SetAttributeValue(Attribute.physicResistance, GetStatusValue());
                break;
            case EquipmentClass.EquipmentType.Boots:
                equip.itemIcon = bootsSprite;
                //itemAttributes.Add("PhysicResistance");
                equip.SetAttributeValue(Attribute.physicResistance, GetStatusValue());
                break;
            case EquipmentClass.EquipmentType.Gloves:
                equip.itemIcon = glovesSprite;
                //itemAttributes.Add("PhysicResistance");
                equip.SetAttributeValue(Attribute.physicResistance, GetStatusValue());
                break;
            case EquipmentClass.EquipmentType.Ring:
                equip.itemIcon = ringSprite;
                //itemAttributes.Add("MagicResistance");
                equip.SetAttributeValue(Attribute.magicResistance, GetStatusValue());
                break;
            case EquipmentClass.EquipmentType.Amulet:
                equip.itemIcon = amuletSprite;
                //itemAttributes.Add("MagicResistance");
                equip.SetAttributeValue(Attribute.magicResistance, GetStatusValue());
                break;
        }

        // Attribute roll based in the equip rarity
        int qtyAttributes = (int)rarity + 1;
        for(int i = 1; i < qtyAttributes; i++)
        {
            EntityAttribute attribute = equip.equipAttributes[Random.Range(0, EntityAttribute.attributeCount)];
            while (attribute.value > 0)
            {
                attribute = equip.equipAttributes[Random.Range(0, EntityAttribute.attributeCount)];
            }
            attribute.value = GetStatusValue();
        }

        equip.GenerateName();
        return equip;
    }

    private int GetStatusValue()
    {
        int value = Random.Range(1, 11);
        return value;
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
