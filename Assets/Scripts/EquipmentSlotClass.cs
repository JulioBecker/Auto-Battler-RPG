using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSlotClass : SlotClass
{
    public EquipmentClass.EquipmentType equipSlotType;
    void Awake()
    {
        quantity = 1;
    }
}
