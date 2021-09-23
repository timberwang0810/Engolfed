using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionsTest : MonoBehaviour
{
    public int Points = 0;
    public OnChangePositionTest holeScript;

    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
        GrowHole();
    }

    private void GrowHole()
    {
        Points++;

        if (Points % 2 == 0)
        {
            StartCoroutine(holeScript.ScaleHoleTest());
        }
    }
}
