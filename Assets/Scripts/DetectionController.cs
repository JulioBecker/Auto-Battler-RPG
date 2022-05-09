using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionController : MonoBehaviour
{
    public GameObject player;

    private void Update()
    {
        transform.position = player.transform.position;
        GetComponent<CircleCollider2D>().radius = player.GetComponent<PlayerController>().detection;
    }
}
