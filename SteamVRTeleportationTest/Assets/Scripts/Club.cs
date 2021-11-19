using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Club : MonoBehaviour
{
    public bool isClipping = false;
    private float yPos;

    private Vector3 initPos;
    private Vector3 currPos;
    private Vector3 v;
    public float maxSpeed;


    private void Start()
    {
        initPos = transform.position;
        currPos = transform.position;
        yPos = transform.position.y;
    }

    private void Update()
    {
        
    }
    

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("YES");

    //    if (collision.gameObject.CompareTag("FloorCollider"))
    //    {
    //        isClipping = true;
    //    }
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("FloorCollider"))
    //    {
    //        isClipping = false;
    //    }
    //}

    // Update is called once per frame
    void FixedUpdate()
    {
        initPos = currPos;
        currPos = transform.position;
        v = (currPos - initPos) / Time.deltaTime;
    }

    public Vector3 GetClubVelocity()
    {
        float speed = v.magnitude;
        //if (speed > maxSpeed) return v * (maxSpeed / speed);
        return v / 200;
    }
}
