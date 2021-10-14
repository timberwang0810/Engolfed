using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class GameManager : MonoBehaviour
{
    public enum GameState { menu, getReady, playing, paused, oops, gameOver };

    public GameState gameState;
    public static GameManager S;

    [System.Serializable]
    public struct SpawnObjectPair
    {
        public GameObject objectPrefab;
        public int objectCount;
    }

    public SpawnObjectPair[] objects;
    public float boundaryPadding = 0.1f;
    public GameObject player;
    public GameObject holeMonster;
    public GameObject[] gameScene;

    // Game Variables
    private int numBalls = 1;
    private int numStrokes;
    private Vector3 playerStartPos;
    private Quaternion playerStartRot;
    private Vector3 holeStartPos;
    private Quaternion holeStartRot;

    private void Awake()
    {
        // Singleton Definition
        if (GameManager.S)
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
        gameState = GameState.menu;
        playerStartPos = player.transform.position;
        playerStartRot = player.transform.rotation;
        holeStartPos = holeMonster.transform.position;
        holeStartRot = holeMonster.transform.rotation;
        StartNewGame();
    }
    public void Reset()
    {
        player.transform.position = playerStartPos;
        player.transform.rotation = playerStartRot;
        holeMonster.transform.position = holeStartPos;
        holeMonster.transform.rotation = holeStartRot;
    }
    public void StartNewGame()
    {
        gameState = GameState.playing;
    }

    public void OnBallSpawned()
    {
        numBalls++;
    }

    public void OnBallCaptured()
    {
        if (gameState != GameState.playing) return;
        numBalls--;
        if (numBalls <= 0) OnGameWon();
    }

    public void OnHoleStruck()
    {
        numStrokes++;
        UIManager.S.UpdateStrokeCount(numStrokes);
    }

    public int GetNumStrokes()
    {
        return numStrokes;
    }

    public void OnGameLost()
    {
        gameState = GameState.gameOver;
        StartCoroutine(LevelTransition());
    }
    private void OnGameWon()
    {
        gameState = GameState.gameOver;
        Debug.Log("HOOOORAY");
        SoundManager.S.MakeYaySound();
    }

    private IEnumerator LevelTransition()
    {
        SteamVR_Fade.Start(Color.black, 3f);
        yield return new WaitForSeconds(3);
        Reset();
        SteamVR_Fade.Start(Color.clear, 1.5f);
        yield return new WaitForSeconds(1.5f);
        StartNewGame();
    }
}
