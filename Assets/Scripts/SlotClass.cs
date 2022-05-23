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
        string color = item.GetColor();
        string text = string.Format("<b><color={0}>{1}</color></b>", color, item.itemName);
        if(item is EquipmentClass)
        {
            EquipmentClass equip = item.GetEquipment();
            EquipmentClass compare = null;

            if (InventoryManager.inventoryManager.GetEquipSlotByType(equip.equipType).item != null) {
                compare = InventoryManager.inventoryManager.GetEquipSlotByType(equip.equipType).item.GetEquipment();
            }
            else
            {
                compare = (EquipmentClass)ScriptableObject.CreateInstance("EquipmentClass");
            }

            text += GenerateStatsText(equip);
            if(!(this is EquipmentSlotClass))
            {
                text += GenerateCompareText(equip, compare);
            }
        }
        return text;
    }

    private string GenerateStatsText(EquipmentClass equip)
    {
        string stats = string.Empty;
        foreach(EntityAttribute attribute in equip.equipAttributes)
        {
            if(attribute.value > 0)
            {
                stats += "\n" + attribute.name + ": " + attribute.value.ToString();
            }
        }
        
        return stats;
    }

    private string GenerateCompareText(EquipmentClass equip, EquipmentClass compare)
    {
        string compareText = "\n\n<b>Ao equipar:</b>";
        for(int i = 1; i < EntityAttribute.attributeCount; i++)
        {
            if(equip.equipAttributes[i].value > 0 || compare.equipAttributes[i].value > 0)
            {
                compareText += "\n" + equip.equipAttributes[i].name + ": " + ReturnStringCompare(equip.equipAttributes[i].value, compare.equipAttributes[i].value);
            }
        }

        return compareText;
    }

    private string ReturnStringCompare(int newAttribute, int oldAttribute)
    {
        if (newAttribute == 0 && oldAttribute == 0) return string.Empty;
        if (newAttribute > oldAttribute) return string.Format("<color=green>(+{0})</color>", newAttribute - oldAttribute);
        if (oldAttribute > newAttribute) return string.Format("<color=red>(-{0})</color>", oldAttribute - newAttribute);
        return "(=)";
    }
}
