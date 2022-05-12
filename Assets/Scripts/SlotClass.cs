using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotClass : MonoBehaviour
{
    public ItemClass item;
    public int quantity;

    public void Clear()
    {
        item = null;
        quantity = 0;
    }
}
