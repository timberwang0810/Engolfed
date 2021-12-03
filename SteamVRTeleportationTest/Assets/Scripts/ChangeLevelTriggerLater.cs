using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Valve.VR;

public class ChangeLevelTriggerLater : MonoBehaviour
{
    public string nextLevelName;
    public bool isLevelCompleted;
    public GameObject player;

    private void Start()
    {
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && isLevelCompleted)
        {
            Debug.Log("IN");
            gameObject.GetComponent<Collider>().enabled = false;
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
        GameObject.Find("LeftHand").SetActive(false);
        SoundManager.S.MakeExitLevelSound();
        SteamVR_Fade.Start(Color.black, 3.0f);
        yield return new WaitForSeconds(5.0f);
        //if (player) Destroy(player);
        SoundManager.S.StopAllSounds();
        SceneManager.LoadScene(nextLevelName);
    }
}
