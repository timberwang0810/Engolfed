using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager S;
    public GameObject menuPanel;
    public GameObject inGameUI;
    public TextMeshProUGUI strokeCount;
    public ARPlaneManager planeManager;

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
        menuPanel.SetActive(true);
        inGameUI.SetActive(false);
        if (planeManager) planeManager.enabled = false;
    }

    public void OnGameStart()
    {
        menuPanel.SetActive(false);
        if (planeManager) planeManager.enabled = true;
        Debug.Log("go!");
        inGameUI.SetActive(true);
    }

    public void UpdateStrokeCount(int stroke)
    {
        strokeCount.text = "Stroke: " + stroke;
    }

}
