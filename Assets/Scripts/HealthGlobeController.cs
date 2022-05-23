using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthGlobeController : MonoBehaviour
{
    [SerializeField] int healthRecover;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            player.Recover(healthRecover, 0);
            player.gameUtils.ShowText(collision.gameObject, "+" + healthRecover.ToString() + " Vida", Color.green);
            Destroy(this.gameObject);
        }
    }
}
