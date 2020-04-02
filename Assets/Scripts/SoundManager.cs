using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance = null;
    private AudioSource[] audios = null;

    private void Awake() {
        if (!Instance)
            Instance = this;
    }

    //  void Start()
    // {
    //     Leons = Resources.Load<AudioClip>("leon");
    //     Mathilda = Resources.Load<AudioClip>("mathilda");
    //     Victory = Resources.Load<AudioClip>("victory");
    //     Dead = Resources.Load<AudioClip>("dead");

    //     audioSrc = GetComponent<AudioSource>();
    // }

    private void Start() {
        audios = GetComponents<AudioSource>();
    }

    public void Play(string audioName)
    {
        AudioSource audio = Array.Find(audios, a => a.clip.name == audioName);

        if (!audio) {
            Debug.LogWarning("SoundManager Can't find audio named " + audioName);
            return;
        }
        audio.Play();
    }

}
