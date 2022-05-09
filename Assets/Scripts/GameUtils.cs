using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUtils : MonoBehaviour
{
    public GameObject floatTextPrefab;

    public bool CheckProbability(float prob)
    {
        return Random.value <= prob;
    }

    public void ShowText(GameObject go, string text, Color color)
    {
        GameObject floatText = Instantiate(floatTextPrefab, go.transform.position + new Vector3(0, 1.5f), Quaternion.identity);
        floatText.GetComponentInChildren<TextMesh>().color = color;
        floatText.GetComponentInChildren<TextMesh>().text = text;
    }
}
