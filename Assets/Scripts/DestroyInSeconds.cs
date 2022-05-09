using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyInSeconds : MonoBehaviour
{
    [SerializeField] float timeToDestroy = 1f;
    void Start()
    {
        Destroy(this.gameObject, timeToDestroy);
    }

}
