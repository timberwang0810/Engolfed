using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Valve.VR;

public class ChangeLevelTrigger : MonoBehaviour
{
    public string nextLevelName;
    public bool isLevelCompleted;
    public GameObject player;
    public Transform holeDestination;
    private Camera playerCam;
    public GameObject hole;
    public GameObject backFence;
    public GameObject logoScreen;

    public GameObject tempPanel;
    public Text tempText;

    private void Start()
    {
        hole.SetActive(false);
        tempPanel.GetComponent<Image>().canvasRenderer.SetAlpha(0);
        playerCam = player.GetComponentInChildren<Camera>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && isLevelCompleted)
        {
            Debug.Log("IN");
            gameObject.GetComponent<Collider>().enabled = false;
            backFence.SetActive(true);
            SoundManager.S.StopMusic();
            StartCoroutine(LevelTransitionOld());
        }
    }
    private IEnumerator LevelTransition()
    {
        SteamVR_Fade.Start(Color.black, 3.0f);
        yield return new WaitForSeconds(5.0f);
        //if (player) Destroy(player);
        SoundManager.S.StopAllSounds();
        SceneManager.LoadScene(nextLevelName);
    }

    private IEnumerator LevelTransitionOld()
    {
        tempText.text = "Now check your score! Don't forget to put the scorecard away also";
        int numScorecarded = TutorialManager.S.numScorecarded;
        tempPanel.GetComponent<Image>().CrossFadeAlpha(1, 2, false);
        yield return new WaitForSeconds(2);
        while (TutorialManager.S.numScorecarded == numScorecarded)
        {
            yield return new WaitForSeconds(0.25f);
        }
        yield return new WaitForSeconds(1.0f);
        tempPanel.SetActive(false);
        // TODO: Hole "animation"
        hole.GetComponent<AudioSource>().Play();
        hole.SetActive(true);
        // Disable teleportation
        RaycastHit hit;
        while (!Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, Mathf.Infinity)
            || !hit.collider.gameObject.CompareTag("Hole"))
        {
            Debug.Log(hit.collider == null ? "null" : hit.collider.gameObject.name);
            yield return new WaitForSeconds(1.0f);
        }
        hole.GetComponent<Collider>().enabled = false;
        hole.GetComponent<AudioSource>().Stop();
        SoundManager.S.MakeScreamSounds();
        SoundManager.S.MakeHoleApproachSound();
        yield return new WaitForSeconds(3.0f);
        SoundManager.S.PlayChargeMusic();
        Vector3 target = new Vector3(holeDestination.position.x, hole.transform.position.y, holeDestination.position.z);
        while (Vector3.Distance(target, hole.transform.position) >= 0.05f)
        {
            hole.transform.position = Vector3.MoveTowards(hole.transform.position, target, 0.1f);
            yield return null;
        }
        SoundManager.S.MakeHoleFrustrationSound();
        GameObject.Find("LeftHand").SetActive(false);
        yield return new WaitForSeconds(3.0f);
        SoundManager.S.MakeExitLevelSound();
        SteamVR_Fade.Start(Color.black, 3.0f);
        yield return new WaitForSeconds(5.0f);
        //if (player) Destroy(player);
        SoundManager.S.StopAllSounds();
        SceneManager.LoadScene(nextLevelName);
    }
}
