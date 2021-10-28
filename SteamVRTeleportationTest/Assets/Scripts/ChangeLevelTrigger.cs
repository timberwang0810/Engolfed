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
    public GameObject hole;

    private void Start()
    {
        hole.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && isLevelCompleted)
        {
            StartCoroutine(LevelTransition());
        }
    }
    private IEnumerator LevelTransition()
    {
        // TODO: Hole "animation"
        SoundManager.S.MakeNomSound();
        hole.SetActive(true);
        // Disable teleportation
        RaycastHit hit;
        while (!Physics.Raycast(player.transform.position, hole.transform.position - player.transform.position, out hit, Mathf.Infinity) 
            || !hit.collider.gameObject.CompareTag("Hole"))
        {
            yield return new WaitForSeconds(1.0f);
            SoundManager.S.MakeNomSound();
        }
        SoundManager.S.MakeHoleApproachSound();
        yield return new WaitForSeconds(3.0f);
        SoundManager.S.PlayChargeMusic();
        while (Vector3.Distance(player.transform.position, hole.transform.position) >= 0.05f)
        {
            hole.transform.position = Vector3.MoveTowards(hole.transform.position, player.transform.position, 5);
            yield return null;
        }
        SteamVR_Fade.Start(Color.black, 3.0f);
        yield return new WaitForSeconds(3.0f);
        //if (player) Destroy(player);
        SoundManager.S.StopAllSounds();
        SceneManager.LoadScene(nextLevelName);
    }
}
