using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OnChangePosition : MonoBehaviour
{
    public PolygonCollider2D hole2DCollider;
    public PolygonCollider2D ground2DCollider;
    public MeshCollider generatedMeshCollider;
    public Collider groundCollider;
    Mesh generatedMesh;

    public float initialScale = 5.0f;
    public Vector3 direction;
    //public float drag = 1.0f;

    const float max_hit_speed = 0.15f;
    const float obstacleCollisionDelta = 0.04f;
    private Vector3 offset;
    private Vector3 lastHolePosition;
    private NavMeshAgent agent;

    public GameObject ground;
    public GameObject patrolPoints;

    [Header("Player Detection")]
    public GameObject player;
    public float sightRadius;
    public float sightAngle;
    public float maxPatrolDelayTime;
    public float patrolSpeed;
    public float chaseSpeed;
    private float currDelay;
   
    private List<Transform> destinations;
    private int currDestination;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //GameObject[] allGameObjects = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        //foreach (var go in allGameObjects)
        //{
        //    if (go.layer == LayerMask.NameToLayer("Obstacles") && go.GetComponent<Collider>() != null)
        //    {
        //        Physics.IgnoreCollision(go.GetComponent<Collider>(), generatedMeshCollider, true);
        //    }
        //}
        hole2DCollider.transform.position = new Vector3(agent.transform.position.x, agent.transform.position.z, 0);
        hole2DCollider.transform.localScale = new Vector3(1, 1, 1) * initialScale;
        lastHolePosition = agent.transform.position;

        destinations = new List<Transform>();
        for (int i = 0; i < patrolPoints.transform.childCount; i++)
        {
            destinations.Add(patrolPoints.transform.GetChild(i));
        }
        currDestination = Random.Range(0, destinations.Count);

        agent.speed = patrolSpeed;
        currDelay = maxPatrolDelayTime;
    }

    private void Update()
    {
        //if (GameManager.S && GameManager.S.gameState != GameManager.GameState.playing) return;

        //Vector3 pos = transform.position;
        //Vector3 groundPos = ground.transform.position;
        //Vector3 groundScale = ground.transform.localScale / 2;
        //float[] groundBoundary = new float[] { groundPos.x - groundScale.x, groundPos.x + groundScale.x, groundPos.z - groundScale.z, groundPos.z + groundScale.z };

        //if ((((pos.x + direction.x * speed) - transform.localScale.x / 2) <= groundBoundary[0]) ||
        //    (((pos.x + direction.x * speed) + transform.localScale.x / 2) >= groundBoundary[1]))
        //{
        //    direction.Set(-1 * direction.x, 0, direction.z);
        //    speed = speed / 2;
        //    if (speed <= drag * Time.deltaTime)
        //    {
        //        speed = drag * Time.deltaTime + 0.001f;
        //    }
        //    //SoundManager.S.MakeBounceSound();
        //}
        //if ((((pos.z + direction.z * speed) - transform.localScale.z / 2) <= groundBoundary[2]) ||
        //    (((pos.z + direction.z * speed) + transform.localScale.z / 2) >= groundBoundary[3]))
        //{
        //    direction.Set(direction.x, 0, -1 * direction.z);
        //    speed = speed / 2;
        //    if (speed <= drag * Time.deltaTime)
        //    {
        //        speed = drag * Time.deltaTime + 0.001f;
        //    }
        //    //SoundManager.S.MakeBounceSound();
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

        //if (Input.GetKey("a"))
        //{
        //    pos.x -= speed * Time.deltaTime;
        //}
        //if (Input.GetKey("d"))
        //{
        //    pos.x += speed * Time.deltaTime;
        //}
        //if (Input.GetKey("s"))
        //{
        //    pos.z -= speed * Time.deltaTime;
        //}
        //if (Input.GetKey("w"))
        //{
        //    pos.z += speed * Time.deltaTime;
        //}

        //transform.position = pos;

        RaycastHit hit;
        if (currDelay <= maxPatrolDelayTime + 1) currDelay += Time.deltaTime;
        //Debug.DrawLine(agent.transform.position, player.transform.position, Color.white);
        Debug.DrawRay(agent.transform.position, player.transform.position - agent.transform.position, Color.white);
        Debug.Log(Vector3.Distance(agent.transform.position, player.transform.position));

        if (Vector3.Distance(agent.transform.position, player.transform.position) <= sightRadius
            && Vector3.Angle(agent.transform.forward, player.transform.position - agent.transform.position) <= sightAngle
            && Physics.Raycast(agent.transform.position, player.transform.position - agent.transform.position, out hit, sightRadius))
        {
            Debug.Log("detected something");
            if (hit.collider.CompareTag("Player"))
            {
                agent.speed = chaseSpeed;
                agent.destination = player.transform.position;
                currDelay = 0;
            }
            else
            {
                agent.speed = patrolSpeed;
                if (currDelay > maxPatrolDelayTime)
                {
                    agent.destination = destinations[currDestination].position;
                    //Debug.Log(Vector3.Distance(agent.transform.position, destinations[currDestination].position));
                    if (Vector3.Distance(agent.transform.position, destinations[currDestination].position) <= 0.1f)
                    {
                        currDestination = Random.Range(0, destinations.Count);
                        currDelay = 0;
                    }
                }
            }
        }

        else
        {
            agent.speed = patrolSpeed;
            if (currDelay > maxPatrolDelayTime)
            {
                agent.destination = destinations[currDestination].position;
                //Debug.Log(Vector3.Distance(agent.transform.position, destinations[currDestination].position));
                if (Vector3.Distance(agent.transform.position, destinations[currDestination].position) <= 0.1f)
                {
                    currDestination = Random.Range(0, destinations.Count);
                    currDelay = 0;
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
            agent.transform.localScale = Vector3.Lerp(startScale, endScale, t);
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
            if ((Mathf.Abs((col_pos.x + col_x_len) - (agent.transform.position.x - hole_x_len)) < obstacleCollisionDelta) ||
                (Mathf.Abs((col_pos.x - col_x_len) - (agent.transform.position.x + hole_x_len)) < obstacleCollisionDelta))
            {
                Debug.Log("flip x");
                direction.Set(-1 * direction.x, 0, direction.z);
                //SoundManager.S.MakeBounceSound();
            }
            if ((Mathf.Abs((col_pos.z + col_z_len) - (agent.transform.position.z - hole_z_len)) < obstacleCollisionDelta) ||
                (Mathf.Abs((col_pos.z - col_z_len) - (agent.transform.position.z + hole_z_len)) < obstacleCollisionDelta))
            {
                Debug.Log("flip z");
                direction.Set(direction.x, 0, -1 * direction.z);
                //SoundManager.S.MakeBounceSound();
            }

            //speed = speed / 2;
            //if (speed <= drag * Time.deltaTime)
            //{
            //    speed = drag * Time.deltaTime + 0.001f;
            //}
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Physics.IgnoreCollision(other, groundCollider, false);
        Physics.IgnoreCollision(other, generatedMeshCollider, true);
    }

    private void FixedUpdate()
    {
        if (agent.transform.hasChanged == true)
        {
            agent.transform.hasChanged = false;
            hole2DCollider.transform.position += new Vector3(agent.transform.position.x - lastHolePosition.x, agent.transform.position.z - lastHolePosition.z, 0);
            hole2DCollider.transform.localScale = transform.localScale * initialScale;

            MakeHole2D();
            Make3DMeshCollider();
            lastHolePosition = agent.transform.position;
        }
    }

    private void MakeHole2D()
    {
        Vector2[] PointPositions = hole2DCollider.GetPath(0);
        for (int i = 0; i < PointPositions.Length; i++)
        {
            PointPositions[i] = hole2DCollider.transform.TransformPoint(PointPositions[i]);
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
