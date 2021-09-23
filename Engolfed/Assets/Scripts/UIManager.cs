using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class UIManager : MonoBehaviour
{
    public static UIManager S;
    public GameObject menuPanel;
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
        if (planeManager) planeManager.enabled = false;
    }

    public void OnGameStart()
    {
        menuPanel.SetActive(false);
        if (planeManager) planeManager.enabled = true;
    }
}
