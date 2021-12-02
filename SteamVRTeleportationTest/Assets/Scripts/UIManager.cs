using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Valve.VR;

public class UIManager : MonoBehaviour
{
    public static UIManager S;
    public GameObject menuPanel;
    public GameObject inGameUI;
    public GameObject deathScreen;
    public GameObject winScreen;
    public GameObject levelScoreScreen;
    public TextMeshProUGUI strokeCount;

    public GameObject scorecardUI;

    private void Awake()
    {
        // Singleton Definition
        if (UIManager.S)
        {
            // singleton exists, delete this object
            Destroy(this.gameObject);
        }
        else
        {
            S = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //menuPanel.SetActive(true);
        //inGameUI.SetActive(false);
        strokeCount.text = "0";
        levelScoreScreen.SetActive(false);
    }

    public void OnGameStart()
    {
        //menuPanel.SetActive(false);
        //Debug.Log("go!");
        //inGameUI.SetActive(true);
        SoundManager.S.MakeStartSound();
    }

    public void UpdateStrokeCount(int stroke)
    {
        strokeCount.text = stroke.ToString();
    }

    public void ShowWinScreen()
    {
        winScreen.SetActive(true);
    }

    public void HideWinScreen()
    {
        winScreen.SetActive(false);
    }

    public void ShowLevelScorecardScreen()
    {
        levelScoreScreen.SetActive(true);
        levelScoreScreen.GetComponentInChildren<TextMeshProUGUI>().text = "Total Stroke:\n" + strokeCount.text;
    }
    //public IEnumerator FadeFromBlack()
    //{
    //    inGameUI.SetActive(false);
    //    Color opaque = new Color(0f, 0f, 0f, 1.0f);
    //    Color transparent = new Color(0f, 0f, 0f, 0.0f);
    //    float timer = 0;
    //    while (timer < 1.5f)
    //    {
    //        timer += Time.deltaTime;
    //        deathScreen.GetComponent<Image>().color = Color.Lerp(opaque, transparent, timer / 1.5f);
    //        yield return null;
    //    }
    //    inGameUI.SetActive(true);
    //}

    //public IEnumerator FadeToBlack()
    //{
    //    Color opaque = new Color(0f, 0f, 0f, 1.0f);
    //    Color transparent = new Color(0f, 0f, 0f, 0f);
    //    float timer = 0;
    //    while (timer < 3.0f)
    //    {
    //        timer += Time.deltaTime;
    //        deathScreen.GetComponent<Image>().color = Color.Lerp(transparent, opaque, timer / 3.0f);
    //        yield return null;
    //    }
    //    GameManager.S.StartNewGame();
    //}
}