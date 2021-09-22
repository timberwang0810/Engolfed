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

    public float initialScale = 0.5f;
    public float speed = 3.0f;
    public Vector3 direction;
    public float drag = 500.0f;

    const float max_hit_speed = 0.15f;
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
        offset = GameObject.Find("Quad").transform.position;
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
        if (GameManager.S && GameManager.S.gameState != GameManager.GameState.playing) return;

        Vector3 pos = transform.position;
        Vector3 groundPos = ground.transform.position;
        Vector3 groundScale = ground.transform.localScale / 2;
        float[] groundBoundary = new float[] { groundPos.x - groundScale.x, groundPos.x + groundScale.x, groundPos.z - groundScale.z, groundPos.z + groundScale.z };

        //if (Input.GetKey("a"))
        //{
        //    direction.Set(-1, 0, 0);
        //    speed = max_hit_speed;

        //}
        //if (Input.GetKey("d"))
        //{
        //    direction.Set(1, 0, 0);
        //    speed = max_hit_speed;
        //}
        //if (Input.GetKey("s"))
        //{
        //    direction.Set(0, 0, -1);
        //    speed = max_hit_speed;
        //}
        //if (Input.GetKey("w"))
        //{
        //    direction.Set(0, 0, 1);
        //    speed = max_hit_speed;
        //}
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector3 dir = Camera.main.gameObject.transform.forward;
        //    direction.Set(dir.x, 0, dir.z);
        //    speed = max_hit_speed;
        //}

        //if (Input.GetKey("r"))
        //{
        //    speed = 1.0f;
        //}

        //if (((pos.x - transform.localScale.x / 2) <= groundBoundary[0]) ||
        //    ((pos.x + transform.localScale.x / 2) >= groundBoundary[1]))
        //{
        //    direction.Set(-1 * direction.x, 0, 0);
        //    if (speed > drag)
        //    {
        //        speed *= 0.5f;
        //    }
        //}
        //if (((pos.z - transform.localScale.z / 2) <= groundBoundary[2]) ||
        //    ((pos.z + transform.localScale.z / 2) >= groundBoundary[3]))
        //{
        //    direction.Set(0, 0, -1 * direction.z);
        //    if (speed > drag)
        //    {
        //        speed *= 0.5f;
        //    }
        //}

        //if (speed > 0)
        //{
        //    if (update_counter % 8 == 0)
        //    {
        //        speed -= drag * Time.deltaTime;
        //        if (speed < 0)
        //        {
        //            speed = 0;
        //        }
        //    }
        //    transform.position += direction * speed;
        //}

        //update_counter++;

        if (Input.GetKey("a"))
        {
            //Debug.Log("pressed");
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

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 dir = Camera.main.gameObject.transform.forward;
            dir.y = 0;
            pos += dir * speed * Time.deltaTime;
        }
        transform.position = pos;
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

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision");
        if (collision.gameObject.name == "house")  // or if(gameObject.CompareTag("YourWallTag"))
        {
            speed = 0.0f;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "house")  // or if(gameObject.CompareTag("YourWallTag"))
        {
            speed = 3.0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered size:" + other.bounds.size + ", hole size: " + hole2DCollider.bounds.size);
        Physics.IgnoreCollision(other, groundCollider, true);
        Physics.IgnoreCollision(other, generatedMeshCollider, false);
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
            hole2DCollider.transform.localScale = new Vector3(1,1,1) * initialScale;
            Debug.Log("hole 2d at: " + hole2DCollider.transform.position);
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
        string msg = "";
        foreach (Vector3 v in PointPositions)
        {
            msg += new Vector3((float)(Mathf.Round(v.x * 100.000f) / 100.000), (float)(Mathf.Round(v.x * 100.000f) / 100.000), (float)(Mathf.Round(v.x * 100.000f) / 100.000)).ToString() + ", ";
        }
        Debug.Log(msg);
        Debug.Log("hole 2d collider: " + hole2DCollider.bounds.size);
        ground2DCollider.pathCount = 2;
        ground2DCollider.SetPath(1, PointPositions);
        msg = "newly drawn: ";
        foreach (Vector3 v in ground2DCollider.GetPath(1))
        {
            msg += v.ToString() + ", ";
        }
        Debug.Log(msg);
        Debug.Log("ground2d collider: " + ground2DCollider.bounds.size);

    }

    private void Make3DMeshCollider()
    {
        if (generatedMesh != null)
        {
            Destroy(generatedMesh);
        }
        generatedMesh = ground2DCollider.CreateMesh(false, false);
        //Debug.Log("generated mesh bound size: " + generatedMesh.bounds.size);
        generatedMeshCollider.sharedMesh = generatedMesh;
    }
}
