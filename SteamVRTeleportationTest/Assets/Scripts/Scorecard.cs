using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scorecard : MonoBehaviour
{
    public GameObject scorecardPrefab;

    private void Start()
    {
        scorecardPrefab = UIManager.S.scorecardUI;
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("triggered enter: " + other.gameObject.name + ", tag: " + other.gameObject.tag);
        if (other.gameObject.CompareTag("Scorecard"))
        {
            scorecardPrefab.SetActive(!scorecardPrefab.activeSelf);
        }
    }
}
