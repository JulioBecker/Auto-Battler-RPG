using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : Interactable
{
    [SerializeField]
    public ItemClass item;
    Vector2 targetPoint;

    private void Start()
    {
        destroyOnInteract = true;
        transform.GetChild(0).GetComponent<TextMesh>().text = string.Format("<color={0}>{1}</color>",  item.GetColor(), item.itemName);
        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = item.itemIcon;
        transform.GetChild(2).localScale = new Vector3(item.itemName.Length * 0.171f, 0.43f, 1);
        targetPoint = new Vector2(transform.position.x + Random.Range(-2f, 2f), transform.position.y + Random.Range(-2f, 2f));
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPoint, 10*Time.deltaTime);
    }

    public override void OnInteract()
    {
        if (InventoryManager.inventoryManager.AddItem(item))
        {

        }
        else
        {
            GameUtils.gameUtils.ShowText(GameUtils.gameUtils.player.gameObject, "Inventário Cheio!", Color.black);
        }
    }
}
