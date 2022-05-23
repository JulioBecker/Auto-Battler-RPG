using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager inventoryManager;
    public GameObject inventoryPanel, equipmentPanel, statsPanel;
    public List<GameObject> slots, equipSlots;
    public ItemClass[] itemsToAdd;
    private SlotClass originalSlot, targetSlot, cursorSlot;
    public GameObject slotHolder, equipSlotHolder, cursor;
    private Image cursorBackground, cursorIcon;
    private int inventorySize, equipSize;
    private bool activeUI = true;
    GameUtils gameUtils;

    private void Awake()
    {
        if(inventoryManager == null)
        {
            inventoryManager = this;
        }
    }

    private void Start()
    {
        gameUtils = FindObjectOfType<GameUtils>();
        ToggleUI();
        cursorBackground = cursor.transform.GetChild(0).GetComponent<Image>();
        cursorBackground.color = Color.clear;
        cursorIcon = cursor.transform.GetChild(1).GetComponent<Image>();
        cursorSlot = cursor.transform.GetChild(1).GetComponent<SlotClass>();
        cursor.SetActive(false);
        inventorySize = slotHolder.transform.childCount;
        equipSize = equipSlotHolder.transform.childCount;
        slots = new List<GameObject>();
        equipSlots = new List<GameObject>();
        for (int i = 0; i < inventorySize; i++)
        {
            slots.Add(slotHolder.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < equipSize; i++)
        {
            equipSlots.Add(equipSlotHolder.transform.GetChild(i).gameObject);
        }

        RefreshEquipmentUI();
        RefreshInventoryUI();

        foreach (ItemClass item in itemsToAdd)
        {
            AddItem(item);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleUI();
        }

        if (activeUI)
        {
            if (Input.GetMouseButtonDown(0))
            {
                originalSlot = GetClosestSlot();
                if (originalSlot == null || originalSlot.item == null) return;
                BeginItemMove();
            }
            if (Input.GetMouseButton(0))
            {
                if (originalSlot == null) return;
                cursor.transform.position = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(0))
            {
                targetSlot = GetClosestSlot();
                if (originalSlot == null || cursorSlot.item == null) return;
                EndItemMove();
            }
        }
    }

    public void ToggleUI()
    {
        activeUI = !activeUI;
        inventoryPanel.SetActive(activeUI);
        equipmentPanel.SetActive(activeUI);
        statsPanel.SetActive(activeUI);
    }

    public void RefreshInventoryUI()
    {
        for (int i = 0; i < inventorySize; i++)
        {
            GameObject slot = slots[i];
            Image background = slot.transform.GetChild(0).GetComponentInChildren<Image>();
            Image slotIcon = slot.transform.GetChild(1).GetComponentInChildren<Image>();
            Text slotText = slot.GetComponentInChildren<Text>();
            SlotClass slotClass = slot.GetComponent<SlotClass>();
            if (slot.GetComponent<SlotClass>().item != null)
            {
                ItemClass item = slotClass.item;
                slotIcon.sprite = item.itemIcon;
                slotIcon.enabled = true;
                switch (item.rarity)
                {
                    case ItemClass.Rarity.common:
                        background.color = Color.gray;
                        break;
                    case ItemClass.Rarity.uncommon:
                        background.color = Color.green;
                        break;
                    case ItemClass.Rarity.rare:
                        background.color = Color.blue;
                        break;
                    case ItemClass.Rarity.epic:
                        background.color = new Color(0.5f, 0, 0.5f, 1); //purple
                        break;
                    case ItemClass.Rarity.legendary:
                        background.color = new Color(1, 0.65f, 0, 1); //orange
                        break;
                }
                if (item.isStackable)
                {
                    slotText.text = slotClass.quantity.ToString();
                }
                else
                {
                    slotText.text = "";
                }
            }
            else
            {
                background.color = Color.clear;
                slotIcon.enabled = false;
                slotText.text = "";
            }
        }
    }

    public void RefreshEquipmentUI()
    {
        for (int i = 0; i < equipSize; i++)
        {
            GameObject equipSlot = equipSlots[i];
            EquipmentSlotClass equipSlotClass = equipSlot.GetComponent<EquipmentSlotClass>();
            Image background = equipSlot.transform.GetChild(0).GetComponentInChildren<Image>();
            Image equipIcon = equipSlot.transform.GetChild(1).GetComponentInChildren<Image>();
            if (equipSlotClass.item != null)
            {
                ItemClass item = equipSlotClass.item;
                switch (item.rarity)
                {
                    case ItemClass.Rarity.common:
                        background.color = Color.gray;
                        break;
                    case ItemClass.Rarity.uncommon:
                        background.color = Color.green;
                        break;
                    case ItemClass.Rarity.rare:
                        background.color = Color.blue;
                        break;
                    case ItemClass.Rarity.epic:
                        background.color = new Color(0.5f, 0, 0.5f, 1); //purple
                        break;
                    case ItemClass.Rarity.legendary:
                        background.color = new Color(1, 0.65f, 0, 1); //orange
                        break;
                }
                equipIcon.sprite = item.itemIcon;
                equipIcon.enabled = true;
            }
            else
            {
                background.color = Color.clear;
                equipIcon.enabled = false;
            }
        }
    }

    public bool AddItem(ItemClass item)
    {
        bool itemAdded;
        if (item.isStackable)
        {
            SlotClass temp = ContainItem(item.itemName);
            if (temp != null)
            {
                temp.quantity++;
                itemAdded = true;
            }
            else
            {
                temp = FreeSlot();
                if (temp != null)
                {
                    temp.item = item;
                    temp.quantity = 1;
                    itemAdded = true;
                }
                else
                {
                    itemAdded = false;
                }
            }
        }
        else
        {
            SlotClass temp = FreeSlot();
            if (temp != null)
            {
                temp.item = item;
                temp.quantity = 1;
                itemAdded = true;
            }
            else
            {
                itemAdded = false;
            }
        }
        RefreshInventoryUI();
        return itemAdded;
    }

    public bool RemoveItem(ItemClass item)
    {
        SlotClass slot = ContainItem(item);
        if (slot != null)
        {
            if (item.isStackable && slot.quantity > 1)
            {
                slot.quantity--;
            }
            else
            {
                slot.Clear();
            }
            RefreshInventoryUI();
        }
        return slot != null;
    }

    private SlotClass ContainItem(string itemName)
    {
        foreach (GameObject slot in slots)
        {
            if (slot.GetComponent<SlotClass>().item != null && slot.GetComponent<SlotClass>().item.itemName == itemName)
            {
                return slot.GetComponent<SlotClass>();
            }
        }
        return null;
    }

    private SlotClass ContainItem(ItemClass item)
    {
        foreach (GameObject slot in slots)
        {
            if (slot.GetComponent<SlotClass>().item == item)
            {
                return slot.GetComponent<SlotClass>();
            }
        }
        return null;
    }

    private SlotClass FreeSlot()
    {
        foreach (GameObject slot in slots)
        {
            if (slot.GetComponent<SlotClass>().item == null)
            {
                return slot.GetComponent<SlotClass>();
            }
        }
        return null;
    }

    private SlotClass GetClosestSlot()
    {
        foreach (GameObject slot in slots)
        {
            if (Vector2.Distance(slot.transform.position, Input.mousePosition) <= 24)
            {
                return slot.GetComponent<SlotClass>();
            }
        }
        foreach (GameObject equipSlot in equipSlots)
        {
            if (Vector2.Distance(equipSlot.transform.position, Input.mousePosition) <= 24)
            {
                return equipSlot.GetComponent<SlotClass>();
            }
        }
        return null;
    }

    public EquipmentSlotClass GetEquipSlotByType(EquipmentClass.EquipmentType equipType)
    {
        foreach (GameObject equipSlot in equipSlots)
        {
            if(equipSlot.GetComponent<EquipmentSlotClass>().equipSlotType == equipType)
            {
                return equipSlot.GetComponent<EquipmentSlotClass>();
            }
        }
        return null;
    }

    private void BeginItemMove()
    {
        cursorSlot.item = originalSlot.item;
        cursorSlot.quantity = originalSlot.quantity;
        originalSlot.Clear();
        if(originalSlot is EquipmentSlotClass)
        {
            ChangePlayerStats(cursorSlot.item.GetEquipment(), null);
        }

        cursorBackground.color = originalSlot.transform.GetChild(0).GetComponentInChildren<Image>().color;
        cursorIcon.GetComponent<Image>().sprite = cursorSlot.item.itemIcon;
        cursor.SetActive(true);

        RefreshInventoryUI();
        RefreshEquipmentUI();
    }

    private void EndItemMove()
    {
        if (targetSlot == null) //no slot drag
        {
            originalSlot.item = cursorSlot.item;
            originalSlot.quantity = cursorSlot.quantity;
            if(originalSlot is EquipmentSlotClass)
            {
                ChangePlayerStats(null, cursorSlot.item.GetEquipment());
            }
        }
        else if (targetSlot is EquipmentSlotClass) //equip item
        {
            EquipItem();
        }
        else if (targetSlot.item == null) //no item in slot
        {
            targetSlot.item = cursorSlot.item;
            targetSlot.quantity = cursorSlot.quantity;
        }
        else //switch items slots
        {
            // switching from equipment slot

            // can't
            if (originalSlot is EquipmentSlotClass && (!(targetSlot.item is EquipmentClass) || !IsSameEquipType(cursorSlot.item.GetEquipment(), targetSlot.item.GetEquipment())))
            {
                originalSlot.item = cursorSlot.item;
                originalSlot.quantity = cursorSlot.quantity;
            }
            else //can
            {
                if (originalSlot is EquipmentSlotClass)
                {
                    ChangePlayerStats(null, targetSlot.item.GetEquipment());
                }

                originalSlot.item = targetSlot.item;
                originalSlot.quantity = targetSlot.quantity;

                targetSlot.item = cursorSlot.item;
                targetSlot.quantity = cursorSlot.quantity;  
            }
        }

        cursorSlot.Clear();
        cursor.SetActive(false);
        RefreshEquipmentUI();
        RefreshInventoryUI();
    }

    private void EquipItem()
    {
        if (cursorSlot.item is EquipmentClass)
        {
            bool sameType = cursorSlot.item.GetEquipment().equipType == targetSlot.GetComponent<EquipmentSlotClass>().equipSlotType;
            if (sameType)
            {
                if (targetSlot.item != null)
                {
                    originalSlot.item = targetSlot.item;
                    originalSlot.quantity = targetSlot.quantity;

                    targetSlot.item = cursorSlot.item;
                    targetSlot.quantity = cursorSlot.quantity;

                    ChangePlayerStats(originalSlot.item.GetEquipment(), targetSlot.item.GetEquipment());
                }
                else
                {
                    targetSlot.item = cursorSlot.item;
                    targetSlot.quantity = cursorSlot.quantity;

                    ChangePlayerStats(null, targetSlot.item.GetEquipment());
                }
                return;
            }
        }
        originalSlot.item = cursorSlot.item;
        originalSlot.quantity = cursorSlot.quantity;
    }

    private bool IsSameEquipType(EquipmentClass equip1, EquipmentClass equip2)
    {
        return equip1.equipType == equip2.equipType;
    }

    private void ChangePlayerStats(EquipmentClass oldItem, EquipmentClass newItem)
    {
        if (oldItem != null)
        {
            foreach(EntityAttribute attribute in oldItem.equipAttributes)
            {
                GameUtils.gameUtils.player.entity.attributes[(int)attribute.attribute].value -= oldItem.equipAttributes[(int)attribute.attribute].value;
            }
            //GameUtils.gameUtils.player.entity.maxHealth -= oldItem.health;
            //GameUtils.gameUtils.player.entity.maxMana -= oldItem.mana;
            //GameUtils.gameUtils.player.entity.attack -= oldItem.attack;
            //GameUtils.gameUtils.player.entity.magic -= oldItem.magic;
            //GameUtils.gameUtils.player.entity.attackSpeed -= oldItem.attackSpeed;
            //GameUtils.gameUtils.player.entity.critChance -= oldItem.critChance;
            //GameUtils.gameUtils.player.entity.critDamage -= oldItem.critDamage;
            //GameUtils.gameUtils.player.entity.dodgeChance -= oldItem.dodgeChance;
            //GameUtils.gameUtils.player.entity.physicResistance -= oldItem.physicResistance;
            //GameUtils.gameUtils.player.entity.magicResistance -= oldItem.magicResistance;
            //GameUtils.gameUtils.player.entity.speed -= oldItem.speed;
            //GameUtils.gameUtils.player.entity.range -= oldItem.range;

            GameUtils.gameUtils.player.Recover(-oldItem.GetAttributeValue(Attribute.maxHealth), -oldItem.GetAttributeValue(Attribute.maxMana));
        }

        if (newItem != null)
        {
            foreach (EntityAttribute attribute in newItem.equipAttributes)
            {
                GameUtils.gameUtils.player.entity.attributes[(int)attribute.attribute].value += newItem.equipAttributes[(int)attribute.attribute].value;
            }
            //GameUtils.gameUtils.player.entity.maxHealth += newItem.health;
            //GameUtils.gameUtils.player.entity.maxMana += newItem.mana;
            //GameUtils.gameUtils.player.entity.attack += newItem.attack;
            //GameUtils.gameUtils.player.entity.magic += newItem.magic;
            //GameUtils.gameUtils.player.entity.attackSpeed += newItem.attackSpeed;
            //GameUtils.gameUtils.player.entity.critChance += newItem.critChance;
            //GameUtils.gameUtils.player.entity.critDamage += newItem.critDamage;
            //GameUtils.gameUtils.player.entity.dodgeChance += newItem.dodgeChance;
            //GameUtils.gameUtils.player.entity.physicResistance += newItem.physicResistance;
            //GameUtils.gameUtils.player.entity.magicResistance += newItem.magicResistance;
            //GameUtils.gameUtils.player.entity.speed += newItem.speed;
            //GameUtils.gameUtils.player.entity.range += newItem.range;

            GameUtils.gameUtils.player.Recover(newItem.GetAttributeValue(Attribute.maxHealth), newItem.GetAttributeValue(Attribute.maxMana));
        }
    }
}
