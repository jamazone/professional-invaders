using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static AudioClip Leons, Mathilda, Dead, Victory;
    static AudioSource audioSrc;

     void Start()
    {
        Leons = Resources.Load<AudioClip>("leon");
        Mathilda = Resources.Load<AudioClip>("mathilda");
        Victory = Resources.Load<AudioClip>("victory");
        Dead = Resources.Load<AudioClip>("dead");

        audioSrc = GetComponent<AudioSource>();
    }

    public static void PlaySound (string clip)
    {
        switch (clip)
        {
            case "leon":
                audioSrc.PlayOneShot(Leons); ;
                break;
            case "mathilda":
                audioSrc.PlayOneShot(Mathilda); ;
                break;
            case "victory":
                audioSrc.PlayOneShot(Victory); ;
                break;
            case "dead":
                audioSrc.PlayOneShot(Dead); ;
                break;
        }
    }
}
