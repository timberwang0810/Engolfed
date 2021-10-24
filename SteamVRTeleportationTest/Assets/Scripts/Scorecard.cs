using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scorecard : MonoBehaviour
{
    public GameObject scorecardPrefab;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("triggered enter: " + other.gameObject.name + ", tag: " + other.gameObject.tag);
        if (other.gameObject.CompareTag("Scorecard"))
        {
            GameObject scorecard = other.gameObject.transform.GetChild(0).gameObject;
            scorecard.SetActive(!scorecard.activeSelf);
        }
    }
}
