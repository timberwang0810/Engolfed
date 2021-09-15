using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class InputManager : MonoBehaviour
{
    public GameObject AR_object;
    public Camera AR_Camera;
    public ARRaycastManager raycastManager;
    public ARPlaneManager planeManager;
    public List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private bool planeLocked;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // TODO: Test plane detection- only detect one plane
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = AR_Camera.ScreenPointToRay(Input.mousePosition);
            if (raycastManager.Raycast(ray, hits))
            {
                
                Pose pose = hits[0].pose;
                Instantiate(AR_object, pose.position, pose.rotation);
            }
            if (!planeLocked)
            {
                if (Physics.Raycast(ray, out RaycastHit raycastHit))
                {
                    TogglePlaneDetection(false, raycastHit.collider.gameObject);
                    planeLocked = true;
                }
            }
        }
    }

    public void TogglePlaneDetection(bool value, GameObject planeToKeep)
    {
        planeManager.enabled = value;

        if (planeManager.enabled)
        {
            SetAllPlanesActive(true, planeToKeep);
        }
        else
        {
            SetAllPlanesActive(false, planeToKeep);
        }
    }

    /// <summary>
    /// Iterates over all the existing planes and activates
    /// or deactivates their <c>GameObject</c>s'.
    /// </summary>
    /// <param name="value">Each planes' GameObject is SetActive with this value.</param>
    void SetAllPlanesActive(bool value, GameObject planeToKeep)
    {
        foreach (var plane in planeManager.trackables)
        {
            if (plane.gameObject.Equals(planeToKeep)) continue;
            plane.gameObject.SetActive(value);

        }
    }

}
