using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class FloorCollider : MonoBehaviour
{

    public GameObject handObject;
    private float yPos;
    private bool isClipping;

    public SteamVR_Action_Vibration hapticAction;
    private Vector3 oldPos;
    private Vector3 oldPosWorld;
    // Start is called before the first frame update
    void Start()
    {
        //yPos = handObject.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        //if (isClipping)
        //{
        //    Vector3 newPos = handObject.transform.position;
        //    newPos.y = yPos;
        //    handObject.transform.position = newPos;
        //}
        //else
        //{
        //    yPos = transform.position.y;
        //}
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Club"))
        {
            GameObject clubHead = other.gameObject.transform.GetChild(0).gameObject;
            oldPos = clubHead.transform.localPosition;
            oldPosWorld = handObject.transform.position;

            Pulse(1, 150, 75);
        }  
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Club"))
        {
            //Club clubScript = other.gameObject.GetComponent<Club>();
            //Debug.Log("YES " + Mathf.Abs(oldPosWorld.y - handObject.transform.position.y));
            GameObject clubHead = other.gameObject.transform.GetChild(0).gameObject;
            clubHead.transform.localPosition = new Vector3(clubHead.transform.localPosition.x,
                                                                    oldPos.y + (Mathf.Abs(oldPosWorld.y- handObject.transform.position.y)* 2.5f),
                                                                    clubHead.transform.localPosition.z);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Club"))
        {
            GameObject clubHead = other.gameObject.transform.GetChild(0).gameObject;
            //clubScript.isClipping = true;
            isClipping = true;
            clubHead.transform.localPosition = oldPos;
        }
    }

    private void Pulse(float duration, float frequency, float amplitude)
    {
        hapticAction.Execute(0, duration, frequency, amplitude, SteamVR_Input_Sources.RightHand);
    }
}
