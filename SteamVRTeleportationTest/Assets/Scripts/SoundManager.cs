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
    public AudioClip holeApproachSound;
    public AudioClip bgm_music;
    public AudioClip charge_music;
    public AudioClip puttSound;

    private AudioSource audio;

    public AudioSource bgm;


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

    public void MakeHoleApproachSound()
    {
        audio.PlayOneShot(holeApproachSound);
    }

    public void PlayChargeMusic()
    {
        bgm.clip = charge_music;
        bgm.Play();
    }

    public void StopMusic()
    {
        bgm.Stop();
    }

    public void PlayBGM()
    {
        bgm.clip = bgm_music;
        bgm.Play();
    }

    public void StopAllSounds()
    {
        StopMusic();
        audio.Stop();
    }

    public void MakePuttSound()
    {
        audio.PlayOneShot(puttSound);
    }
}
