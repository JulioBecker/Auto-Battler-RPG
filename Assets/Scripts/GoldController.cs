using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldController : MonoBehaviour
{
    int goldQty;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<PlayerController>().CollectGold(goldQty);
            Destroy(this.gameObject);
        }
    }

    public void setGoldQty(int qty)
    {
        goldQty = qty;
        string text = qty == 1 ? " Gold" : " Golds";
        GetComponentInChildren<TextMesh>().text = qty.ToString() + text;
    }
}
