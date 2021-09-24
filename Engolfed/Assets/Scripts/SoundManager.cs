using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager S;

    public AudioClip winSound;
    public AudioClip bounceSound;
    public AudioClip wooshSound;
    public AudioClip nomSound;
    public AudioClip yaySound;
    public AudioClip startSound;

    private AudioSource audio;
    private void Awake()
    {
        if (SoundManager.S)
        {
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
        audio = GetComponent<AudioSource>();
    }

    public void MakeWinSound()
    {
        audio.PlayOneShot(winSound);
    }

    public void MakeBounceSound()
    {
        audio.PlayOneShot(bounceSound);
    }

    public void MakeWooshSound()
    {
        audio.PlayOneShot(wooshSound);
    }

    public void MakeNomSound()
    {
        audio.PlayOneShot(nomSound);
    }

    public void MakeYaySound()
    {
        audio.PlayOneShot(yaySound);
    }

    public void MakeStartSound()
    {
        audio.PlayOneShot(startSound);
    }
}
