using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;
public class TutorialManager : MonoBehaviour
{
    public static TutorialManager S;
    public GameObject golfSwingPanel;
    public GameObject scorecardPanel;
    public GameObject teleportationPanel;

    public bool hasSwung = false;
    public bool hasScorecarded = false;

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
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBallStruck()
    {
        if (hasSwung) return;
        hasSwung = true;
        StartCoroutine(PanelTransitionCoroutine(golfSwingPanel, scorecardPanel));
    }

    public void OnScorecardShown()
    {
        if (hasScorecarded) return;
        hasScorecarded = true;
        StartCoroutine(PanelTransitionCoroutine(scorecardPanel, teleportationPanel));
        teleport.LoadTutorialPanel(teleportationPanel);
    }
}
