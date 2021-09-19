using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnChangePosition : MonoBehaviour
{
    public PolygonCollider2D hole2DCollider;
    public PolygonCollider2D ground2DCollider;
    public MeshCollider generatedMeshCollider;
    public Collider groundCollider;
    Mesh generatedMesh;

    public float initialScale = 0.5f;
    public float speed = 3.0f;

    public GameObject ground;



    private void Start()
    {
        GameObject[] allGameObjects = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach (var go in allGameObjects)
        {
            if(go.layer == LayerMask.NameToLayer("Obstacles"))
            {
                Physics.IgnoreCollision(go.GetComponent<Collider>(), generatedMeshCollider, true);
            }
        }
    }

    private void Update()
    {
        Vector3 pos = transform.position;
        Vector3 groundPos = ground.transform.position;
        Vector3 groundScale = ground.transform.localScale / 2;
        float[] groundBoundary = new float[] {groundPos.x - groundScale.x, groundPos.x + groundScale.x, groundPos.z - groundScale.z, groundPos.z + groundScale.z};

        if (Input.GetKey("a") && (pos.x - transform.localScale.x/2) > groundBoundary[0])
        {
            pos.x -= speed * Time.deltaTime;
        }
        if (Input.GetKey("d") && (pos.x + transform.localScale.x/2) < groundBoundary[1])
        {
            pos.x += speed * Time.deltaTime;
        }
        if (Input.GetKey("s") && (pos.z - transform.localScale.z/2) > groundBoundary[2])
        {
            pos.z -= speed * Time.deltaTime;
        }
        if (Input.GetKey("w") && (pos.z + transform.localScale.z/2) < groundBoundary[3])
        {
            pos.z += speed * Time.deltaTime;
        }

        if (Input.GetKey("r"))
        {
            speed = 3.0f;
        }

        transform.position = pos;
    }

    public IEnumerator ScaleHole()
    {
        Vector3 startScale = transform.localScale;
        Vector3 endScale = startScale * 2;

        float t = 0;
        while(t <= 0.4)
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
        if(transform.hasChanged == true)
        {
            transform.hasChanged = false;
            hole2DCollider.transform.position = new Vector2(transform.position.x, transform.position.z);
            hole2DCollider.transform.localScale = transform.localScale * initialScale;
            MakeHole2D();
            Make3DMeshCollider();
        }
    }

    private void MakeHole2D()
    {
        Vector2[] PointPositions = hole2DCollider.GetPath(0);

        for(int i = 0; i < PointPositions.Length; i++)
        {
            PointPositions[i] = hole2DCollider.transform.TransformPoint(PointPositions[i]);
        }

        ground2DCollider.pathCount = 2;
        ground2DCollider.SetPath(1, PointPositions);
    }

    private void Make3DMeshCollider()
    {
        if(generatedMesh != null)
        {
            Destroy(generatedMesh);
        }
        generatedMesh = ground2DCollider.CreateMesh(true, true);
        generatedMeshCollider.sharedMesh = generatedMesh;
    }
}
