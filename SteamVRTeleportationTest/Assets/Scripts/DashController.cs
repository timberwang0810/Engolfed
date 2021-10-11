using System.Collections;
using UnityEngine;
/*
[RequireComponent(typeof(SteamVR_TrackedController))]
public class DashController : MonoBehaviour
{
    [SerializeField]
    private float minDashRange = 0.5f;
    [SerializeField]
    private float maxDashRange = 40f;
    [SerializeField]
    private float dashTime = 0.2f;

    [SerializeField]
    private Animator maskAnimator;

    private SteamVR_TrackedController trackedController;
    private Transform cameraRigRoot;

    private void Start()
    {
        cameraRigRoot = transform.parent;

        trackedController = GetComponent<SteamVR_TrackedController>();
        trackedController.PadClicked += TryDash;
    }

    private void TryDash(object sender, ClickedEventArgs e)
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.distance > minDashRange && hit.distance < maxDashRange)
            {
                StartCoroutine(DoDash(hit.point));
            }
        }
    }


    private IEnumerator DoDash(Vector3 endPoint)
    {
        if (maskAnimator != null)
            maskAnimator.SetBool("Mask", true);

        yield return new WaitForSeconds(0.1f);

        float elapsed = 0f;

        Vector3 startPoint = cameraRigRoot.position;

        while (elapsed < dashTime)
        {
            elapsed += Time.deltaTime;
            float elapsedPct = elapsed / dashTime;

            cameraRigRoot.position = Vector3.Lerp(startPoint, endPoint, elapsedPct);
            yield return null;
        }

        if (maskAnimator != null)
            maskAnimator.SetBool("Mask", false);
    }
}*/