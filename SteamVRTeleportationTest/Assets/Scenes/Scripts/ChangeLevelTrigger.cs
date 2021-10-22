using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;

public class ChangeLevelTrigger : MonoBehaviour
{
    public string nextLevelName;
    public bool isLevelCompleted;
    public GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && isLevelCompleted)
        {
            StartCoroutine(LevelTransition());
        }
    }
    private IEnumerator LevelTransition()
    {
        SteamVR_Fade.Start(Color.black, 3.0f);
        yield return new WaitForSeconds(3.0f);
        if (player) Destroy(player);
        SceneManager.LoadScene(nextLevelName);
    }
}
