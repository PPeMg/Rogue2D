using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public AudioSource musicSource;
    public AudioSource sfxSource;

    public float lowPitchRange = 0.95f;
    public float highPitchRange = 1.05f;

    //To make it a Singleton
    public static SoundManager instance;

    private void Awake()
    {
        if(SoundManager.instance == null)
        {
            SoundManager.instance = this;
        } else if (SoundManager.instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void PlaySingle(AudioClip sound)
    {
        sfxSource.clip = sound;
        sfxSource.pitch = 1f;
        sfxSource.Play();
    }

    public void RandomizeSFX(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);


        sfxSource.clip = clips[randomIndex];
        sfxSource.pitch = randomPitch;
        sfxSource.Play();
    }
}
