using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackableObject : MonoBehaviour
{
    GameUtils gameUtils;

    [SerializeField] GameObject goldPrefab;
    [Range(0f, 1f)] [SerializeField] float goldChance;
    [SerializeField] int goldMin;
    [SerializeField] int goldMax;

    private void Start()
    {
        gameUtils = FindObjectOfType<GameUtils>();
    }
    public void OnAttacked()
    {
        if(this.tag == "Breakable")
        {
            Destroy(this.gameObject);
            if (gameUtils.CheckProbability(goldChance))
            {
                int goldQty = Random.Range(goldMin, goldMax + 1);
                GameObject gold = Instantiate(goldPrefab, transform.position, Quaternion.identity);
                gold.GetComponent<GoldController>().setGoldQty(goldQty);
            }
        }
    }
}
