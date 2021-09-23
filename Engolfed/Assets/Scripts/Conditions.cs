using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conditions : MonoBehaviour
{
    public int Points = 0;
    public OnChangePositionTest holeScript;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            GameManager.S.OnBallCaptured();
        }
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
