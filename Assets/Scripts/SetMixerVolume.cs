using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetMixerVolume : MonoBehaviour
{
    [SerializeField]
    private AudioMixer mixer = null;
    [SerializeField]
    private string volumeParameterName = "";

    public void SetVolume(float value)
    {
        mixer.SetFloat(volumeParameterName, Mathf.Log10(value) * 20);
    }
}
