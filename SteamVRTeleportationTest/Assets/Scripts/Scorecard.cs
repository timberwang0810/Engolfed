using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Scorecard : MonoBehaviour
{
    public GameObject scorecardPrefab;
    public Hand hand;

    private bool hasGrabbed;

    private void Start()
    {
        scorecardPrefab = UIManager.S.scorecardUI;
    }
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("triggered enter: " + other.gameObject.name + ", tag: " + other.gameObject.tag);
        if (hasGrabbed) return;
        if (other.gameObject.CompareTag("Scorecard"))
        {
            Debug.Log(hand.GetGrabStarting());
            if (hand.GetGrabStarting() != GrabTypes.None)
            {
                if (!scorecardPrefab.activeSelf)
                {
                    SoundManager.S.MakeScorecardSound();
                    TutorialManager.S?.OnScorecardShown();
                }
                scorecardPrefab.SetActive(true);
                hasGrabbed = true;
            }
            else
            {
                if (scorecardPrefab.activeSelf)
                {
                    SoundManager.S.MakeScorecardSound();
                    TutorialManager.S?.OnScorecardReturned();
                }
                scorecardPrefab.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Scorecard")) hasGrabbed = false;

    }
}
