using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXScript : MonoBehaviour
{
    [Range(0,50f)]public float screenShake;
    AudioSource[] sounds;
    ParticleSystem[] particles;
    bool keepAlive = true;

    void Awake()
    {
        sounds = GetComponentsInChildren<AudioSource>();
        particles = GetComponentsInChildren<ParticleSystem>();

        foreach (AudioSource source in sounds)
        {
            if (source.loop) source.loop=false;
        }

        ScreenShake.AddShake(screenShake);
    }

    void Update()
    {
        keepAlive = false;

        foreach (AudioSource source in sounds)
        {
            if (source.isPlaying) keepAlive=true;
        }

        foreach (ParticleSystem system in particles)
        {
            if (system.isPlaying) keepAlive=true;
        }

        if (keepAlive==false) Destroy(this.gameObject);
        
    }

} // FIN DU SCRIPT
