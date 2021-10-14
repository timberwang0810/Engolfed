using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillerCollider : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            GameManager.S.OnBallCaptured();
        }
        else
        {
            SoundManager.S.MakeNomSound();
        }
        Destroy(collision.gameObject);
    }
}
