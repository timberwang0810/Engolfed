using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conditions : MonoBehaviour
{
    public int Points = 0;
    public OnChangePosition holeScript;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            Destroy(other.gameObject);
            SoundManager.S.MakeWinSound();
            GameManager.S.OnBallCaptured();
        }
        else
        {
            Destroy(other.gameObject);
            GrowHole();
        }
    }

    private void GrowHole()
    {
        Points++;

        if (Points % 2 == 0)
        {
            StartCoroutine(holeScript.ScaleHole());
        }

        SoundManager.S.MakeNomSound();
    }
}
