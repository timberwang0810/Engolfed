using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conditions : MonoBehaviour
{
    public int Points = 0;
    public OnChangePosition holeScript;
    public SoundManager soundManager;

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

        soundManager.MakeNomSound();

        if (Points % 2 == 0)
        {
            StartCoroutine(holeScript.ScaleHole());
        }
    }
}
