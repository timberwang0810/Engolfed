using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;
using TMPro;
public class TutorialManager : MonoBehaviour
{
    public static TutorialManager S;
    public GameObject golfSwingPanel;
    public GameObject scorecardPanel;
    public GameObject returnCardPanel;
    public GameObject teleportationPanel;

    public bool hasSwung = false;
    public bool hasScorecarded = false;
    public bool hasScorecardReturned = false;

    private bool isTransitioning = false;
    public int numScorecarded = 0;

    public Teleport teleport;

    private void Awake()
    {
        if (TutorialManager.S) Destroy(this.gameObject);
        else S = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PanelTransitionCoroutine(golfSwingPanel, golfSwingPanel));
    }

    private IEnumerator PanelTransitionCoroutine(GameObject from, GameObject to)
    {
        isTransitioning = true;
        if (from.activeSelf)
        {
            from.GetComponent<Image>().CrossFadeAlpha(0, 2, false);
            yield return new WaitForSeconds(2);
            from.SetActive(false);
        }
        yield return new WaitForSeconds(1.5f);
        to.GetComponent<Image>().canvasRenderer.SetAlpha(0);
        to.SetActive(true);
        to.GetComponent<Image>().CrossFadeAlpha(1, 2, false);
        isTransitioning = false;
        //from.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBallStruck()
    {
        if (hasSwung || isTransitioning) return;
        hasSwung = true;
        StartCoroutine(PanelTransitionCoroutine(golfSwingPanel, scorecardPanel));
    }

    public void OnScorecardShown()
    {
        numScorecarded++;
        if (!hasSwung || hasScorecarded || isTransitioning) return;
        hasScorecarded = true;
        StartCoroutine(PanelTransitionCoroutine(scorecardPanel, returnCardPanel));
    }

    public void OnScorecardReturned()
    {
        if (!hasSwung || !hasScorecarded || hasScorecardReturned || isTransitioning) return;
        hasScorecardReturned = true;
        StartCoroutine(PanelTransitionCoroutine(returnCardPanel, teleportationPanel));
        teleport.LoadTutorialPanel(teleportationPanel);
    }
}
