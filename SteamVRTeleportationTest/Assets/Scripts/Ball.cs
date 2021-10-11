using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float force = 3;
    public float speed = 1.0f;
    public float drag = 1.0f;

    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (speed > 0) {
            speed -= drag * Time.deltaTime;
            Debug.Log(GetComponent<Rigidbody>().velocity);
        }
        speed -= drag * Time.deltaTime;
        if (speed < 0)
        {
            speed = 0;
        }
        transform.position += direction * speed;
}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Club"))
        {
            //Vector3 dir = collision.contacts[0].point - transform.position;
            //dir = -dir.normalized;
            //StartCoroutine(TurnOffClubCollision(collision));
            //GetComponent<Rigidbody>().AddForce(dir * force);
            
            //GetComponent<Rigidbody>().velocity = other.gameObject.GetComponent<Club>().GetClubVelocity();
            Vector3 vel_vec = other.gameObject.GetComponent<Club>().GetClubVelocity();
            speed = vel_vec.magnitude;
            direction = vel_vec / speed;
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
