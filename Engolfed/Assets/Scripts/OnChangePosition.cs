using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;

public class OnChangePosition : MonoBehaviour
{
    public PolygonCollider2D hole2DCollider;
    public PolygonCollider2D ground2DCollider;
    public MeshCollider generatedMeshCollider;
    public Collider groundCollider;
    Mesh generatedMesh;

    public ARRaycastManager raycastManager;
    public float initialScale = 5.0f;
    public float speed = 1.0f;
    public Vector3 direction;
    public float drag = 1.0f;

    const float max_hit_speed = 0.15f;
    const float obstacleCollisionDelta = 0.04f;
    private Vector3 offset;
    private Vector3 lastHolePosition; 

    public GameObject ground;
    int update_counter = 0;


    private void Start()
    {
        GameObject[] allGameObjects = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach (var go in allGameObjects)
        {
            if (go.layer == LayerMask.NameToLayer("Obstacles") && go.GetComponent<Collider>() != null)
            {
                Physics.IgnoreCollision(go.GetComponent<Collider>(), generatedMeshCollider, true);
            }
        }
        hole2DCollider.transform.position = transform.position;
        hole2DCollider.transform.localScale = new Vector3(1, 1, 1) * initialScale;
        offset = GameObject.Find("Ground").transform.position;
        lastHolePosition = transform.position;
    }

    public IEnumerator SpawnObjects(GameObject plane, GameManager.SpawnObjectPair[] objects, float boundaryPadding)
    {
        //plane.gameObject.transform.localScale = new Vector3(plane.size.x / 10.00f, 0.1f, plane.size.y / 10.00f);
        Debug.Log("plane scale: " + plane.gameObject.transform.localScale);
        Debug.Log("plane mask " + plane.gameObject.layer);
        Debug.Log("plane bounds min" + plane.GetComponent<Renderer>().bounds.min + " max " + plane.GetComponent<Renderer>().bounds.max);
        yield return new WaitForSeconds(1);
        foreach (GameManager.SpawnObjectPair pair in objects)
        {
            //float scale = (plane.transform.size.x * plane.size.y);
            for (int i = 0; i < pair.objectCount; i++)
            {
                GameObject obj = Instantiate(pair.objectPrefab, new Vector3(Random.Range(plane.GetComponent<Renderer>().bounds.min.x + boundaryPadding, plane.GetComponent<Renderer>().bounds.max.x - boundaryPadding),
                    plane.transform.position.y + 0.1f,
                    Random.Range(plane.GetComponent<Renderer>().bounds.min.z + boundaryPadding, plane.GetComponent<Renderer>().bounds.max.z - boundaryPadding)),
                    Quaternion.identity);
                //obj.transform.localScale *= 10;
                //obj.transform.parent = null;
                obj.GetComponent<Rigidbody>().useGravity = false;
                if (obj.layer == LayerMask.NameToLayer("Obstacles") && obj.GetComponent<Collider>() != null)
                {
                    Debug.Log("name: " + obj.name);
                    Physics.IgnoreCollision(obj.GetComponent<Collider>(), generatedMeshCollider, true);
                }
                Debug.Log("position of " + obj.name + ": " + obj.transform.position);
                if (obj.CompareTag("Ball")) GameManager.S.OnBallSpawned();
                obj.GetComponent<Rigidbody>().useGravity = true;
            }
        }
        plane.transform.GetChild(0).gameObject.SetActive(true);
    }

    private void Update()
    {
        //if (GameManager.S && GameManager.S.gameState != GameManager.GameState.playing) return;

        Vector3 pos = transform.position;
        Vector3 groundPos = ground.transform.position;
        Vector3 groundScale = ground.transform.localScale / 2;
        float[] groundBoundary = new float[] { groundPos.x - groundScale.x, groundPos.x + groundScale.x, groundPos.z - groundScale.z, groundPos.z + groundScale.z };

        if ((((pos.x + direction.x * speed) - transform.localScale.x / 2) <= groundBoundary[0]) ||
            (((pos.x + direction.x * speed) + transform.localScale.x / 2) >= groundBoundary[1]))
        {
            direction.Set(-1 * direction.x, 0, direction.z);
            speed = speed / 2;
            if (speed <= drag * Time.deltaTime)
            {
                speed = drag * Time.deltaTime + 0.001f;
            }
            SoundManager.S.MakeBounceSound();
        }
        if ((((pos.z + direction.z * speed) - transform.localScale.z / 2) <= groundBoundary[2]) ||
            (((pos.z + direction.z * speed) + transform.localScale.z / 2) >= groundBoundary[3]))
        {
            direction.Set(direction.x, 0, -1 * direction.z);
            speed = speed / 2;
            if (speed <= drag * Time.deltaTime)
            {
                speed = drag * Time.deltaTime + 0.001f;
            }
            SoundManager.S.MakeBounceSound();
        }

        if (speed > 0)
        {
            if (update_counter % 8 == 0)
            {
                speed -= drag * Time.deltaTime;
                if (speed < 0)
                {
                    speed = 0;
                }
            }
            transform.position += direction * speed;
        }

        update_counter++;

        if (Input.GetKey("a"))
        {
            pos.x -= speed * Time.deltaTime;
        }
        if (Input.GetKey("d"))
        {
            pos.x += speed * Time.deltaTime;
        }
        if (Input.GetKey("s"))
        {
            pos.z -= speed * Time.deltaTime;
        }
        if (Input.GetKey("w"))
        {
            pos.z += speed * Time.deltaTime;
        }


        //if (Input.GetKey("r"))
        //{
        //    speed = 3.0f;
        //}

        if (Input.GetMouseButtonDown(0) && speed <= 0)
        {
            var touch = Input.GetTouch(0);
            List<ARRaycastHit> arRaycastHits = new List<ARRaycastHit>();
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            if (Physics.Raycast(ray, out RaycastHit raycastHit))
            {
                if (raycastHit.collider.gameObject.CompareTag("Hole"))
                {
                    Vector3 dir = Camera.main.gameObject.transform.forward;
                    direction.Set(dir.x, 0, dir.z);

                    speed = 0.04f;

                    SoundManager.S.MakeWooshSound();
                    GameManager.S.OnHoleStruck();
                }
            }
        }
    }

    public IEnumerator ScaleHole()
    {
        Vector3 startScale = transform.localScale;
        Vector3 endScale = startScale * 2;

        float t = 0;
        while (t <= 0.4)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered size:" + other.bounds.size + ", hole size: " + hole2DCollider.bounds.size);
        Physics.IgnoreCollision(other, groundCollider, true);
        Physics.IgnoreCollision(other, generatedMeshCollider, false);

        float col_x_len = other.gameObject.GetComponent<Collider>().bounds.extents.x;
        float col_z_len = other.gameObject.GetComponent<Collider>().bounds.extents.z;
        Vector3 col_pos = other.gameObject.GetComponent<Collider>().bounds.center;

        float hole_x_len = GetComponent<Collider>().bounds.extents.x; //transform.localScale.x;// 
        float hole_z_len = GetComponent<Collider>().bounds.extents.z;

        if ((col_x_len + obstacleCollisionDelta > hole_x_len) ||
           (col_z_len + obstacleCollisionDelta > hole_z_len))
        {
            if ((Mathf.Abs((col_pos.x + col_x_len) - (transform.position.x - hole_x_len)) < obstacleCollisionDelta) ||
                (Mathf.Abs((col_pos.x - col_x_len) - (transform.position.x + hole_x_len)) < obstacleCollisionDelta))
            {
                Debug.Log("flip x");
                direction.Set(-1 * direction.x, 0, direction.z);
                SoundManager.S.MakeBounceSound();
            }
            if ((Mathf.Abs((col_pos.z + col_z_len) - (transform.position.z - hole_z_len)) < obstacleCollisionDelta) ||
                (Mathf.Abs((col_pos.z - col_z_len) - (transform.position.z + hole_z_len)) < obstacleCollisionDelta))
            {
                Debug.Log("flip z");
                direction.Set(direction.x, 0, -1 * direction.z);
                SoundManager.S.MakeBounceSound();
            }

            speed = speed / 2;
            if (speed <= drag * Time.deltaTime)
            {
                speed = drag * Time.deltaTime + 0.001f;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Physics.IgnoreCollision(other, groundCollider, false);
        Physics.IgnoreCollision(other, generatedMeshCollider, true);
    }

    private void FixedUpdate()
    {
        if (transform.hasChanged == true)
        {
            transform.hasChanged = false;
            hole2DCollider.transform.position += new Vector3(transform.position.x - lastHolePosition.x, transform.position.z - lastHolePosition.z, 0);
            hole2DCollider.transform.localScale = transform.localScale * initialScale;

            MakeHole2D();
            Make3DMeshCollider();
            lastHolePosition = transform.position;
        }
    }

    private void MakeHole2D()
    {
        Vector2[] PointPositions = hole2DCollider.GetPath(0);
        for (int i = 0; i < PointPositions.Length; i++)
        {
            PointPositions[i] = hole2DCollider.transform.TransformPoint(PointPositions[i])-offset;
        }
        /*string msg = "";
        foreach (Vector3 v in PointPositions)
        {
            msg += new Vector3((float)(Mathf.Round(v.x * 100.000f) / 100.000), (float)(Mathf.Round(v.x * 100.000f) / 100.000), (float)(Mathf.Round(v.x * 100.000f) / 100.000)).ToString() + ", ";
        }*/

        ground2DCollider.pathCount = 2;
        ground2DCollider.SetPath(1, PointPositions);
        /*msg = "newly drawn: ";
        foreach (Vector3 v in ground2DCollider.GetPath(1))
        {
            msg += v.ToString() + ", ";
        }*/
    }

    private void Make3DMeshCollider()
    {
        if (generatedMesh != null)
        {
            Destroy(generatedMesh);
        }
        generatedMesh = ground2DCollider.CreateMesh(false, false);
        generatedMeshCollider.sharedMesh = generatedMesh;
    }
}
