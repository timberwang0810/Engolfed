using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float force = 1e-4f;
    public float speed = 0.0f;
    public float drag = 0.01f;
    public float floorHeight = -0.3f;
    public float speedScale = 10.0f;

    public GameObject flag;

    public UIManager UI;
    private int num_strokes = 1;

    private Vector3 direction;

    private List<Vector3> ballPos = new List<Vector3>();
    private LineRenderer lineRenderer;

    private bool wasOOB = false;

    public GameObject clownGate;
    private Animator anim;

    private bool levelComplete = false;
    public bool laterLevel = false;

    // Start is called before the first frame update
    void Start()
    {
        Color red = Color.red;
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        //lineRenderer.startColor = red;
        //lineRenderer.endColor = red;
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;

        if (clownGate) anim = clownGate.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //direction.x = 1.0f;
        /*if (speed > 0)
        {
            speed -= drag * Time.deltaTime;
            Debug.Log(speed);
        }
        if (speed < 1e-4)
        {
            speed = 0;
            Debug.Log(speed);
        }
        else
        {
            transform.position += direction * speed;
            if(transform.position.y < floorHeight)
            {
                transform.position = new Vector3(transform.position.x, floorHeight, transform.position.z);
            }
            Debug.Log(transform.position);
        }*/

        //Store ball positions somewhere
        if (GetComponent<Rigidbody>().velocity.magnitude != 0 && !wasOOB)
        {
            ballPos.Add(transform.position);
        }

        //Change how many points based on the mount of positions is the List
        lineRenderer.positionCount = ballPos.Count;

        for (int i = 0; i < ballPos.Count; i++)
        {
            //Change the postion of the lines
            lineRenderer.SetPosition(i, ballPos[i]);
        }

        // Near flag check
        if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(flag.transform.position.x, flag.transform.position.z)) <= 1)
        {
            //if (GameManager.S) GameManager.S.OnBallCaptured();
            //else if (!levelComplete)
            //{
            //    levelComplete = true;
            //    if(!laterLevel)
            //    {
            //        GameObject.Find("ColliderHolder").GetComponent<ChangeLevelTrigger>().isLevelCompleted = true;
            //    }
            //    else
            //    {
            //        GameObject.Find("ColliderHolder").GetComponent<ChangeLevelTriggerLater>().isLevelCompleted = true;
            //    }
            //    UIManager.S.ShowLevelScorecardScreen();
            //    GameObject.Find("ColliderHolder").GetComponent<AudioSource>().Play();
            //    anim.SetTrigger("Open");
            //    SoundManager.S.MakeChipInSounds();
            //    GameObject.Find("backFence").SetActive(false);
            //}
            //Destroy(this.gameObject, 0.1f);
        }
        // OOB check
        else if (transform.position.y < floorHeight && ballPos.Count > 0)
        {
            Debug.Log("OOB CHECK");
            wasOOB = true;
            SoundManager.S.MakeOOBSound();

            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            transform.position = ballPos[0];
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ClubHead"))
        {
            wasOOB = false;
            SoundManager.S.MakePuttSound();
            TutorialManager.S?.OnBallStruck();

            num_strokes += 1;
            UI.UpdateStrokeCount(num_strokes);

            /*Vector3 vel_vec = other.gameObject.GetComponent<Club>().GetClubVelocity();
            speed = vel_vec.magnitude;
            direction = vel_vec / speed;

            direction = direction.normalized;
            StartCoroutine(TurnOffClubCollision(other));
            GetComponent<Rigidbody>().AddForce(vel_vec * force, ForceMode.Impulse);*/

            GetComponent<Rigidbody>().velocity = other.gameObject.GetComponent<Club>().GetClubVelocity();
            Vector3 vel_vec = other.gameObject.GetComponent<Club>().GetClubVelocity();
            speed = vel_vec.magnitude;
            direction = vel_vec / speed;
            direction.y = 0;

            vel_vec *= speedScale;
            GetComponent<Rigidbody>().velocity = vel_vec;

            ballPos.Clear();
        }
        else if (other.gameObject.CompareTag("BallCapture"))
        {
            if (GameManager.S) GameManager.S.OnBallCaptured();
            else if (!levelComplete)
            {
                levelComplete = true;
                if (!laterLevel)
                {
                    GameObject.Find("ColliderHolder").GetComponent<ChangeLevelTrigger>().isLevelCompleted = true;
                }
                else
                {
                    GameObject.Find("ColliderHolder").GetComponent<ChangeLevelTriggerLater>().isLevelCompleted = true;
                }
                UIManager.S.ShowLevelScorecardScreen();
                GameObject.Find("ColliderHolder").GetComponent<AudioSource>().Play();
                anim.SetTrigger("Open");
                SoundManager.S.MakeChipInSounds();
                GameObject.Find("backFence").SetActive(false);
            }
        }
    } 

    /*private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.CompareTag("ClubHead"))
        {
            wasOOB = false;
            SoundManager.S.MakePuttSound();
            TutorialManager.S?.OnBallStruck();

            num_strokes += 1;
            UI.UpdateStrokeCount(num_strokes);

            Vector3 dir = collision.contacts[0].point - transform.position;
            dir = -dir.normalized;
            StartCoroutine(TurnOffClubCollision(collision));
            GetComponent<Rigidbody>().AddForce(dir * force);

            ballPos.Clear();
        }
    }*/

    private IEnumerator TurnOffClubCollision(Collider other)
    {
        other.gameObject.GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(0.1f);
        other.gameObject.GetComponent<Collider>().enabled = true;
    }
}
