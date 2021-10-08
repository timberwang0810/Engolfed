using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float force = 3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Club"))
        {
            //Vector3 dir = collision.contacts[0].point - transform.position;
            //dir = -dir.normalized;
            //StartCoroutine(TurnOffClubCollision(collision));
            //GetComponent<Rigidbody>().AddForce(dir * force);
            GetComponent<Rigidbody>().velocity = other.gameObject.GetComponent<Club>().GetClubVelocity();
            Debug.Log(GetComponent<Rigidbody>().velocity);
        }
    }

    private IEnumerator TurnOffClubCollision(Collision collision)
    {
        collision.gameObject.GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(2);
        collision.gameObject.GetComponent<Collider>().enabled = true;
    }
}
