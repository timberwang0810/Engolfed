using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Club : MonoBehaviour
{
    private Vector3 initPos;
    private Vector3 currPos;
    private Vector3 v;
    public float maxSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        initPos = transform.position;
        currPos = transform.position;
    }

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
        if (speed > maxSpeed) return v * (maxSpeed / speed);
        return v;
    }
}
