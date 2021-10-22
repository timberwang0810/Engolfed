using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

namespace Valve.VR.InteractionSystem
{
    public class PuttTeleport : MonoBehaviour
    {
        // a reference to the action
        public SteamVR_Action_Boolean SphereOnOff;
        // a reference to the hand
        public SteamVR_Input_Sources handType;
        // a reference to the sphere
        //public GameObject Sphere; 

        public Camera VRCamera;
        private Player player;
        private Vector3 hmdLookDir;

        public Teleport teleport;
        public float ballOffsetScale = 0.6f;

        void Start()
        {
            player = Player.instance;

            SphereOnOff.AddOnStateDownListener(TriggerDown, handType);
            SphereOnOff.AddOnStateUpListener(TriggerUp, handType);
        }
        public void TriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            //Debug.Log("Trigger is up");
            //Sphere.GetComponent<MeshRenderer>().enabled = false;
        }
        public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            //Debug.Log("Trigger is down");
            //Sphere.GetComponent<MeshRenderer>().enabled = true;
            hmdLookDir = VRCamera.transform.forward;
            teleport.TeleportBehindBall(hmdLookDir, ballOffsetScale);
            Debug.Log(hmdLookDir.ToString());
        }
        void Update()
        {
        }
    }
}