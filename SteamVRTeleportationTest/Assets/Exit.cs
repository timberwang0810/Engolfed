using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    private bool doorOpen = false;
    private bool lookingAtDoor = false;
    private bool effectDone = false;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GameObject.Find("doorWing").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Exit check
        Camera hmdCam = GameObject.Find("VRCamera").GetComponent<Camera>();
        Vector3 hmdLookDir = hmdCam.transform.forward;
        //Debug.Log(hmdLookDir);

        if (!doorOpen && (hmdLookDir.z <= -0.9 && hmdLookDir.x <= 0.2)) //doorOpen &&
        {
            lookingAtDoor = true;
        }
        else if (doorOpen && (hmdLookDir.z > -0.7 && Mathf.Abs(hmdLookDir.x) > 0.3))
        {
            lookingAtDoor = false;
            doorOpen = false;
            StartCoroutine(CloseDoor());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ClubHead") && lookingAtDoor)
        {
            if (effectDone)
            {
                Debug.Log("Quit");
                Application.Quit();
            }
            else {
                doorOpen = true;
                StartCoroutine(OpenDoor());
            }
        }
    }

    private IEnumerator OpenDoor()
    {
        anim.SetTrigger("Open");
        yield return new WaitForSeconds(0.4f);
        SoundManager.S.MakeExitDoorOpenSound();
        effectDone = true;
        Debug.Log(effectDone);
    }

    private IEnumerator CloseDoor()
    {
        anim.SetTrigger("Close");
        yield return new WaitForSeconds(0.7f);
        SoundManager.S.MakeExitDoorCloseSound();
        effectDone = false;
    }
}
