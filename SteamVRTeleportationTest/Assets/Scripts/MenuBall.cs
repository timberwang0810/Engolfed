using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;

public class MenuBall : MonoBehaviour
{
    public float force = 1e-4f;
    public float speed = 0.0f;
    public float drag = 0.01f;
    public float floorHeight = -0.3f;
    public float speedScale = 10.0f;

    public GameObject flag;
    public string nextLevelName;
    public GameObject ballLight;

    private int num_strokes = 1;

    private Vector3 direction;

    private List<Vector3> ballPos = new List<Vector3>();
    private LineRenderer lineRenderer;

    private bool wasOOB = true;

    private Animator anim;

    private bool levelComplete = false;
    private bool doorOpen = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //Store ball positions somewhere
        if (GetComponent<Rigidbody>().velocity.magnitude != 0 && !wasOOB)
        {
            ballPos.Add(transform.position);
        }

        // Near flag check
        if (transform.position.z > flag.transform.position.z)
        {
            if (GameManager.S) GameManager.S.OnBallCaptured();
            else if (!levelComplete)
            {
                ballLight.SetActive(false);
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                levelComplete = true;
                StartCoroutine(LevelTransition());
            }
        }
        // OOB check
        else if (!wasOOB && GetComponent<Rigidbody>().velocity.magnitude != 0 && GetComponent<Rigidbody>().velocity.magnitude < 0.2)
        {
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

            GetComponent<Rigidbody>().velocity = other.gameObject.GetComponent<Club>().GetClubVelocity();
            Vector3 vel_vec = other.gameObject.GetComponent<Club>().GetClubVelocity();
            speed = vel_vec.magnitude;
            direction = vel_vec / speed;
            direction.y = 0;

            vel_vec *= speedScale;
            GetComponent<Rigidbody>().velocity = vel_vec;

            ballPos.Clear();
        }
    } 

    private IEnumerator TurnOffClubCollision(Collider other)
    {
        other.gameObject.GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(0.1f);
        other.gameObject.GetComponent<Collider>().enabled = true;
    }

    private IEnumerator LevelTransition()
    {
        //SoundManager.S.MakeHoleFrustrationSound();
        GameObject.Find("LeftHand").SetActive(false);
        yield return new WaitForSeconds(1.0f);
        SoundManager.S.MakeExitLevelSound();
        SteamVR_Fade.Start(Color.black, 3.0f);
        yield return new WaitForSeconds(7.0f);
        SoundManager.S.StopAllSounds();
        SceneManager.LoadScene(nextLevelName);
    }
}
