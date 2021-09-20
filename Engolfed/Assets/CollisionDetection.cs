using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.layer == 8)
        {
            Debug.Log("HIT OBStacle" + collision.gameObject.name);
        }
        else
        {
            Debug.Log("HIT " + collision.gameObject.name + " " + collision.gameObject.layer);
        }
    }
}
