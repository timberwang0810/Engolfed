using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void btn_Test()
    {
        Debug.Log("YASSSSS");
    }

    public void btn_StartGame()
    {
        UIManager.S.OnGameStart();
        if (GameManager.S) GameManager.S.gameState = GameManager.GameState.getReady;
    }

    public void btn_RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

