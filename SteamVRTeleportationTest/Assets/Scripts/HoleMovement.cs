using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HoleMovement : MonoBehaviour
{
    private Vector3 lastHolePosition;
    private NavMeshAgent agent;

    public GameObject patrolPoints;

    [Header("Player Detection")]
    public GameObject player;
    public GameObject ball;
    public float sightRadius;
    public float sightAngle;
    public float maxPatrolDelayTime;
    public float patrolSpeed;
    public float chaseSpeed;
    public float spookCooldown;
    private float currDelay;
    private float currSpookCooldown;

    private List<Transform> destinations;
    private int currDestination;
    private bool didHitPlayer;
    private bool isTracking;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        lastHolePosition = agent.transform.position;

        destinations = new List<Transform>();
        for (int i = 0; i < patrolPoints.transform.childCount; i++)
        {
            destinations.Add(patrolPoints.transform.GetChild(i));
        }
        currDestination = Random.Range(0, destinations.Count);

        agent.speed = patrolSpeed;
        currDelay = maxPatrolDelayTime;
        currSpookCooldown = spookCooldown;
    }

    private void Update()
    {
        if (GameManager.S && GameManager.S.gameState != GameManager.GameState.playing) return;

        RaycastHit hit;
        if (currDelay <= maxPatrolDelayTime + 1) currDelay += Time.deltaTime;
        if (currSpookCooldown <= spookCooldown + 1) currSpookCooldown += Time.deltaTime;

        if (Physics.Raycast(agent.transform.position, player.transform.position - agent.transform.position, out hit, sightRadius * 2))
        {
            if (hit.collider.CompareTag("Player"))
            {
                if (isTracking)
                {
                    if (agent.speed == patrolSpeed)
                    {
                        SoundManager.S.StopMusic();
                        SoundManager.S.PlayChargeMusic();
                    }
                    agent.speed = chaseSpeed;
                    agent.destination = player.transform.position;
                    currDelay = 0;
                }
                else
                {
                    if (!didHitPlayer && currSpookCooldown >= spookCooldown)
                    {
                        SoundManager.S.MakeHoleApproachSound();
                        currSpookCooldown = 0;
                    }
                    didHitPlayer = true;
                    if (Vector3.Distance(agent.transform.position, player.transform.position) <= sightRadius
                        && Vector3.Angle(agent.transform.forward, player.transform.position - agent.transform.position) <= sightAngle)
                    {
                        isTracking = true;
                    }
                    else
                    {
                        if (agent.speed == chaseSpeed)
                        {
                            SoundManager.S.StopMusic();
                            SoundManager.S.PlayBGM();
                        }
                        agent.speed = patrolSpeed;
                        if (currDelay >= maxPatrolDelayTime)
                        {
                            agent.destination = destinations[currDestination].position;
                            if (Vector3.Distance(agent.transform.position, destinations[currDestination].position) <= 0.2f)
                            {
                                currDestination = Random.Range(0, destinations.Count);
                                currDelay = 0;
                            }
                        }
                    }
                }

            }
            else
            {
                didHitPlayer = false;
                isTracking = false;
                if (agent.speed == chaseSpeed)
                {
                    SoundManager.S.StopMusic();
                    SoundManager.S.PlayBGM();
                }
                agent.speed = patrolSpeed;
                if (currDelay >= maxPatrolDelayTime)
                {
                    agent.destination = destinations[currDestination].position;
                    if (Vector3.Distance(agent.transform.position, destinations[currDestination].position) <= 2.0f)
                    {
                        currDestination = Random.Range(0, destinations.Count);
                        currDelay = 0;
                    }
                }
            }
        }

        else
        {
            didHitPlayer = false;
            isTracking = false;
            if (agent.speed == chaseSpeed)
            {
                SoundManager.S.StopMusic();
                SoundManager.S.PlayBGM();
            }
            agent.speed = patrolSpeed;
            if (currDelay >= maxPatrolDelayTime)
            {
                agent.destination = destinations[currDestination].position;
                if (Vector3.Distance(agent.transform.position, destinations[currDestination].position) <= 1.0f)
                {
                    currDestination = Random.Range(0, destinations.Count);
                    currDelay = 0;
                }
            }
        }

        if (Vector3.Distance(agent.transform.position,
            new Vector3(player.transform.position.x, agent.transform.position.y, player.transform.position.z)) <= 0.5f)
        {
            GameManager.S.OnGameLost();
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.S && GameManager.S.gameState != GameManager.GameState.playing) return;

        if (agent.transform.hasChanged == true)
        {
            agent.transform.hasChanged = false;
            lastHolePosition = agent.transform.position;
        }
    }
}