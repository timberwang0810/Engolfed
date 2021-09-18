using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

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
    public float boundaryPadding = 0.15f;
    private ARPlane plane;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartNewGame(GameObject planeObj)
    {
        plane = planeObj.GetComponent<ARPlane>();
        Debug.Log(plane.size);
        Debug.Log("center: " + plane.center);

        SpawnObjects();
    }
    private void SpawnObjects()
    {
        //plane.gameObject.transform.localScale = new Vector3(plane.size.x / 10.00f, 0.1f, plane.size.y / 10.00f);
        Debug.Log("plane scale: " + plane.gameObject.transform.localScale);
        foreach (SpawnObjectPair pair in objects)
        {
            float scale = (plane.size.x * plane.size.y);
            for (int i = 0; i < pair.objectCount; i++)
            {
                GameObject obj = Instantiate(pair.objectPrefab, new Vector3(Random.Range((-plane.size.x/2) + boundaryPadding, (plane.size.x/2) - boundaryPadding) + plane.center.x,
                    plane.center.y,
                    Random.Range((-plane.size.y/2) + boundaryPadding, (plane.size.y/2) - boundaryPadding) + plane.center.z),
                    Quaternion.identity, plane.gameObject.transform);
                obj.transform.localScale *= scale;
                obj.transform.parent = null;
                Debug.Log("position of " + obj.name + ": " + obj.transform.position);
            }
        }
    }
}
