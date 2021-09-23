using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnChangePositionTest : MonoBehaviour
{
    public PolygonCollider2D hole2DCollider;
    public PolygonCollider2D ground2DCollider;
    public MeshCollider generatedMeshCollider;
    public Collider groundCollider;
    Mesh generatedMesh;

    public float initialScale = 5.0f;
    public float speed = 0.0f;
    public Vector3 direction;
    public float drag = 1.0f;

    const float max_hit_speed = 0.05f; //0.15
    const float obstacleCollisionDelta = 0.05f;

    public GameObject ground;

    int update_counter = 0;

    public SoundManager soundManager;



    private void Start()
    {
        GameObject[] allGameObjects = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach (var go in allGameObjects)
        {
            if(go.layer == LayerMask.NameToLayer("Obstacles") && go.GetComponent<Collider>() != null)
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
        float[] groundBoundary = new float[] { groundPos.x - groundScale.x, groundPos.x + groundScale.x, groundPos.z - groundScale.z, groundPos.z + groundScale.z };


        if (Input.GetKey("a") && speed == 0)
        {
            direction.Set(-1, 0, 0);
            speed = max_hit_speed;
        }
        if (Input.GetKey("d") && speed == 0)
        {
            direction.Set(1, 0, 0);
            speed = max_hit_speed;
        }
        if (Input.GetKey("s") && speed == 0)
        {
            direction.Set(0, 0, -1);
            speed = max_hit_speed;
        }
        if (Input.GetKey("w") && speed == 0)
        {
            direction.Set(0, 0, 1);
            speed = max_hit_speed;
        }

        if (Input.GetKey("r"))
        {
            speed = 1.0f;
        }

        if((((pos.x + direction.x * speed) - transform.localScale.x / 2) <= groundBoundary[0]) ||
            (((pos.x + direction.x * speed) + transform.localScale.x / 2) >= groundBoundary[1]))
        {
            direction.Set(-1 * direction.x, 0, 0);
            speed = speed / 2;
            if(speed <= drag * Time.deltaTime)
            {
                speed = drag * Time.deltaTime + 0.01f;
            }
            soundManager.MakeBounceSound();
        }
        if ((((pos.z + direction.z * speed) - transform.localScale.z / 2) <= groundBoundary[2]) ||
            (((pos.z + direction.z * speed) + transform.localScale.z / 2) >= groundBoundary[3]))
        {
            direction.Set(0, 0, -1 * direction.z);
            speed = speed / 2;
            if (speed <= drag * Time.deltaTime)
            {
                speed = drag * Time.deltaTime + 0.01f;
            }
            soundManager.MakeBounceSound();
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

        // TODO: Add GameManager.S.OnHoleStruck(); to update stroke counts
    }

    public IEnumerator ScaleHoleTest()
    {
        Vector3 startScale = transform.localScale;
        Vector3 endScale = startScale * 2;
        hole2DCollider.transform.localScale = hole2DCollider.transform.localScale * 2;

        float t = 0;
        while(t <= 0.4)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Physics.IgnoreCollision(other, groundCollider, true);
        Physics.IgnoreCollision(other, generatedMeshCollider, false);


        float col_x_len = other.gameObject.GetComponent<Collider>().bounds.extents.x;
        float col_z_len = other.gameObject.GetComponent<Collider>().bounds.extents.z;

        float hole_x_len = GetComponent<Collider>().bounds.extents.x;
        float hole_z_len = GetComponent<Collider>().bounds.extents.z;

        if ((col_x_len + obstacleCollisionDelta > hole_x_len) ||
           (col_z_len + obstacleCollisionDelta > hole_z_len))
        {
            Debug.Log("collision!");
            Debug.Log(other.name);
            speed = 0.0f;
        }
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
            Debug.Log("hole 2d at: " + hole2DCollider.transform.position);
            MakeHole2DTest();
            Make3DMeshColliderTest();
        }
    }

    private void MakeHole2DTest()
    {
        Vector2[] PointPositions = hole2DCollider.GetPath(0);

        for(int i = 0; i < PointPositions.Length; i++)
        {
            PointPositions[i] = hole2DCollider.transform.TransformPoint(PointPositions[i]);
        }

        ground2DCollider.pathCount = 2;
        ground2DCollider.SetPath(1, PointPositions);
    }

    private void Make3DMeshColliderTest()
    {
        if(generatedMesh != null)
        {
            Destroy(generatedMesh);
        }
        //Debug.Log("generated mesh bound size: " + ground2DCollider.bounds.size);
        generatedMesh = ground2DCollider.CreateMesh(true, true);
        //Debug.Log("generated mesh bound size: " + generatedMesh.bounds.size);
        generatedMeshCollider.sharedMesh = generatedMesh;
    }
}
