using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotClass : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ItemClass item;
    public int quantity;

    public void Clear()
    {
        item = null;
        quantity = 0;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
        {
            GameUtils.gameUtils.ShowTooltip(TooltipTextConstruct(), transform.position + new Vector3(25, 0));
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameUtils.gameUtils.HideTooltip();
    }

    private string TooltipTextConstruct()
    {
        string color = string.Empty;

        switch (item.rarity)
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
        string text = string.Format("<b><color={0}>{1}</color></b>", color, item.itemName);
        if(item is EquipmentClass)
        {
            EquipmentClass equip = item.GetEquipment();
            if(equip.health > 0)
            {
                text += "\nVida: " + equip.health.ToString();
            }
            if (equip.mana > 0)
            {
                text += "\nMana: " + equip.mana.ToString();
            }
            if (equip.attack > 0)
            {
                text += "\nAtaque: " + equip.attack.ToString();
            }
            if (equip.magic > 0)
            {
                text += "\nMagia: " + equip.magic.ToString();
            }
            if (equip.attackSpeed > 0)
            {
                text += "\nVelocidade de Ataque: " + (Mathf.RoundToInt(equip.attackSpeed * 100)).ToString() + "%";
            }
            if (equip.critChance > 0)
            {
                text += "\nChance de Crítico: " + (Mathf.RoundToInt(equip.critChance * 100)).ToString() + "%";
            }
            if (equip.critDamage > 0)
            {
                text += "\nMultiplicador Crítico: " + (Mathf.RoundToInt(equip.critDamage * 100)).ToString() + "%";
            }
            if (equip.dodgeChance > 0)
            {
                text += "\nChance de Evasão: " + (Mathf.RoundToInt(equip.dodgeChance * 100)).ToString() + "%";
            }
            if (equip.physicResistance > 0)
            {
                text += "\nResistência Física: " + equip.physicResistance.ToString();
            }
            if (equip.magicResistance > 0)
            {
                text += "\nResistência Mágica: " + equip.magicResistance.ToString();
            }
            if (equip.speed > 0)
            {
                text += "\nVelocidade: " + equip.speed.ToString();
            }
            if (equip.range > 0)
            {
                text += "\nAlcance: " + equip.range.ToString();
            }
        }
        return text;
    }
}
