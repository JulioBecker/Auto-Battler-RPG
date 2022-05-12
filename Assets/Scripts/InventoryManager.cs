using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject slotHolder, equipSlotHolder, cursor;
    public GameObject inventoryPanel, equipmentPanel, statsPanel;
    public List<GameObject> slots, equipSlots;
    public ItemClass[] itemsToAdd;
    private SlotClass originalSlot, targetSlot, cursorSlot;
    private int inventorySize, equipSize;
    private bool activeUI = true;

    private void Start()
    {
        ToggleUI();
        cursorSlot = cursor.GetComponent<SlotClass>();
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

        foreach(ItemClass item in itemsToAdd)
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

        if (activeUI) {
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
            Image slotIcon = slot.transform.GetChild(0).GetComponentInChildren<Image>();
            Text slotText = slot.GetComponentInChildren<Text>();
            SlotClass slotClass = slot.GetComponent<SlotClass>();
            if (slot.GetComponent<SlotClass>().item != null)
            {
                ItemClass item = slotClass.item;
                slotIcon.sprite = item.itemIcon;
                slotIcon.enabled = true;
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
            Image equipIcon = equipSlot.transform.GetChild(0).GetComponentInChildren<Image>();
            if (equipSlotClass.item != null)
            {
                ItemClass item = equipSlotClass.item;
                equipIcon.sprite = item.itemIcon;
                equipIcon.enabled = true;
            }
            else
            {
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
                if(temp != null)
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
        if(slot != null)
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
        foreach(GameObject slot in slots)
        {
            if(slot.GetComponent<SlotClass>().item != null && slot.GetComponent<SlotClass>().item.itemName == itemName)
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
            if(slot.GetComponent<SlotClass>().item == null)
            {
                return slot.GetComponent<SlotClass>();
            }
        }
        return null;
    }

    private SlotClass GetClosestSlot()
    {
        foreach(GameObject slot in slots)
        {
            if(Vector2.Distance(slot.transform.position, Input.mousePosition) <= 24)
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

    private void BeginItemMove()
    {   
        cursorSlot.item = originalSlot.item;
        cursorSlot.quantity = originalSlot.quantity;
        originalSlot.Clear();

        cursor.GetComponent<Image>().sprite = cursorSlot.item.itemIcon;
        cursor.SetActive(true);

        RefreshInventoryUI();
        RefreshEquipmentUI();
    }

    private void EndItemMove()
    {
        if(targetSlot == null) //no slot drag
        {
            originalSlot.item = cursorSlot.item;
            originalSlot.quantity = cursorSlot.quantity;
        }
        else if(targetSlot is EquipmentSlotClass) //equip item
        {
            EquipItem();
        }
        else if(targetSlot.item == null) //no item in slot
        {
            targetSlot.item = cursorSlot.item;
            targetSlot.quantity = cursorSlot.quantity;
        }
        else //switch items slots
        {
            if (originalSlot is EquipmentSlotClass && (!(targetSlot.item is EquipmentClass) || !isSameEquipType(cursorSlot.item.GetEquipment(), targetSlot.item.GetEquipment())))
            {
                originalSlot.item = cursorSlot.item;
                originalSlot.quantity = cursorSlot.quantity;
            }
            else
            {
                //isSameEquipType(originalSlot.item.GetEquipment(), targetSlot.item.GetEquipment())
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
        if(cursorSlot.item is EquipmentClass)
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
                }
                else
                {
                    targetSlot.item = cursorSlot.item;
                    targetSlot.quantity = cursorSlot.quantity;
                }
                return;
            }
        }
        originalSlot.item = cursorSlot.item;
        originalSlot.quantity = cursorSlot.quantity;
    }

    private bool isSameEquipType(EquipmentClass equip1, EquipmentClass equip2)
    {
        return equip1.equipType == equip2.equipType;
    }
}
