using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillerCollider : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            if (GameManager.S) GameManager.S.OnBallCaptured();
            else GameObject.Find("clown_door").GetComponent<ChangeLevelTrigger>().isLevelCompleted = true;
        }
        else
        {
            SoundManager.S.MakeNomSound();
        }
        Destroy(collision.gameObject);
    }
}
