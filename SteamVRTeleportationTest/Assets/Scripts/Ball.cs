using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float force = 3;
    public float speed = 0.0f;
    public float drag = 0.01f;
    public float floorHeight = 0.04f;

    public UIManager UI;
    private int num_strokes = 1;

    private Vector3 direction;

    private List<Vector3> ballPos = new List<Vector3>();
    private LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        Color red = Color.red;
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        //lineRenderer.startColor = red;
        //lineRenderer.endColor = red;
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;
    }

    // Update is called once per frame
    void Update()
    {
        //direction.x = 1.0f;
        if (speed > 0)
        {
            speed -= drag * Time.deltaTime;
            Debug.Log(speed);
        }
        if (speed < 1e-4)
        {
            speed = 0;
            Debug.Log(speed);
        }
        else
        {
            transform.position += direction * speed;
            if(transform.position.y < floorHeight)
            {
                transform.position = new Vector3(transform.position.x, floorHeight, transform.position.z);
            }
            Debug.Log(transform.position);
        }

        //Store ball positions somewhere
        if (GetComponent<Rigidbody>().velocity.magnitude != 0)
        {
            ballPos.Add(transform.position);
        }

        //Change how many points based on the mount of positions is the List
        lineRenderer.positionCount = ballPos.Count;

        for (int i = 0; i < ballPos.Count; i++)
        {
            //Change the postion of the lines
            lineRenderer.SetPosition(i, ballPos[i]);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Club"))
        {
            SoundManager.S.MakePuttSound();

            num_strokes += 1;
            UI.UpdateStrokeCount(num_strokes);

            //Vector3 dir = collision.contacts[0].point - transform.position;
            //dir = -dir.normalized;
            //StartCoroutine(TurnOffClubCollision(collision));
            //GetComponent<Rigidbody>().AddForce(dir * force);

            //GetComponent<Rigidbody>().velocity = other.gameObject.GetComponent<Club>().GetClubVelocity();
            Vector3 vel_vec = other.gameObject.GetComponent<Club>().GetClubVelocity();
            speed = vel_vec.magnitude;
            direction = vel_vec / speed;
            direction.y = 0;

            ballPos.Clear();
        }
    }

    private IEnumerator TurnOffClubCollision(Collision collision)
    {
        collision.gameObject.GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(2);
        collision.gameObject.GetComponent<Collider>().enabled = true;
    }
}
